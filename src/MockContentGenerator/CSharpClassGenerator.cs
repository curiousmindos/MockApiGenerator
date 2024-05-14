using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Dynamic;
using System.Text;

namespace MockApi.ContentGenerator
{
    public class CSharpClassGenerator
    {
        private const string propertyTemplate = "\tpublic {0} {1} {{get; set;}}\n";
        private const string collectionTemplate = "\tpublic ICollection<{0}> {1} {{get; set;}}\n";

        public StringBuilder CodeGenerate(string jsonSchemaContent, bool isPascalCase = true, bool isIncludeSystemNamespaces = true)
        {
            dynamic contentObject = JsonConvert.DeserializeObject(jsonSchemaContent)!;


            StringBuilder code = new StringBuilder();
            // namespaces ?
            if (isIncludeSystemNamespaces)
            {
                code.AppendLine("using System;");
                code.AppendLine("using System.Collections.Generic;");
                code.AppendLine("using System.Text;");
                code.AppendLine();
                code.AppendLine($"namespace CodeGenerator.{contentObject.title};\n");
            }

            // root class
            code.AppendLine($"public class {contentObject.title}");
            code.AppendLine("{");

            // properties
            foreach (var item in (IEnumerable<dynamic>)contentObject.properties)
            {
                ClassPropertiesCodeGenerator(isPascalCase, item, code);
            }
            code.AppendLine("}");

            var definitions = contentObject.definitions;
            foreach (JToken definition in definitions)
            {
                code.AppendLine("");
                code.AppendLine($"public class {definition.Path.Substring(12)}");
                code.AppendLine("{");

                // properties
                var properties = definition.Children()["properties"].FirstOrDefault();
                foreach (dynamic item in properties.Children())
                {
                    var propName = item.Name;
                    var propTypeValue = item.Value;
                    var propType = propTypeValue.type;
                    ClassPropertiesCodeGenerator(isPascalCase, item, code);
                }
                code.AppendLine("}");
            }

            // references classes
            return code;
        }

        private void ClassPropertiesCodeGenerator(bool isPascalCase, dynamic item, StringBuilder code)
        {

            var propName = item.Name;
            if (isPascalCase)
            {
                propName = char.ToUpper(propName[0]) + propName.Substring(1);
            }
            var value = item.Value;
            if (item.HasValues)
            {
                var type = value.type;
                var format = value.format;
                var @ref = value["$ref"];
                if (@ref is not null)
                {
                    var refName = @ref.Value;
                    code.AppendFormat(propertyTemplate, refName.Substring(14), propName);
                }
                else
                {
                    // nullable property ?
                    if (type is JArray)
                    {
                        var isNull = ((JArray)type).FirstOrDefault(c => c.ToString() == "null");
                        var nullableType = ((JArray)type).FirstOrDefault(c => c.ToString() != "null");

                        code.AppendFormat(propertyTemplate, $"{ConvertToCrtType(nullableType!.ToString(), format)}?", propName);
                    }
                    // collection ?
                    else if (type == "array")
                    {
                        var array = new List<dynamic>();
                        var items = value.items;
                        var @refItems = items["$ref"];
                        if (@refItems is not null)
                        {
                            //$ref type
                            code.AppendFormat(collectionTemplate, @refItems.Value.Substring(14), propName);
                        }
                        else
                        {
                            // system types
                            var arrayItemType = items["type"];
                            var arrayItemFormat = items["format"];
                            code.AppendFormat(collectionTemplate, ConvertToCrtType(arrayItemType, arrayItemFormat), propName);
                        }
                    }
                    else
                    {
                        // single property
                        code.AppendFormat(propertyTemplate, ConvertToCrtType(type, format), propName.ToString());
                    }
                }
            }
        }


        private string ConvertToCrtType(dynamic itemType, dynamic? itemFormat)
        {
            if (itemFormat is not null)
            {
                var formatType = itemFormat.ToString() switch
                {
                    "date-time" => "DateTime", //DateTimeOffSet ? 
                    "int32" => "Int32",
                    "int16" => "Int16",
                    _ => throw new NotImplementedException(),
                };
                return formatType;
            }

            var formatSingleType = itemType.ToString() switch
            {
                "string" => "string",
                "boolean" => "bool",
                "integer" => "int",
                _ => throw new NotImplementedException(),
            };
            return formatSingleType;
        }
    }
}
