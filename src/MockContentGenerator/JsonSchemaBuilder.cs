﻿using Bogus;
using Newtonsoft.Json;
using System.Dynamic;

namespace MockApi.ContentGenerator;

public class JsonSchemaBuilder
{
    private readonly Faker _faker;

    public JsonSchemaBuilder()
    {
        _faker = new Faker();
    }

    public (string jsonSerializedContent, ExpandoObject buildObject) Build(string jsonContent)
    {
        dynamic buildObject = new ExpandoObject();
        dynamic contentObject = JsonConvert.DeserializeObject(jsonContent)!;
        var definitions = (IEnumerable<dynamic>)contentObject.definitions;
        dynamic definitionsObjects = definitions != null ?  ParseDefinitions(definitions) : default!;
        foreach (var item in (IEnumerable<dynamic>)contentObject.properties)
        {
            AddValueProperty(buildObject, item, definitionsObjects);
        }
        var jsonSerializedContent = JsonConvert.SerializeObject(buildObject);
        return (jsonSerializedContent, buildObject);
    }

    private void AddValueProperty(dynamic buildObject, dynamic item, IDictionary<string, dynamic> definitionsObjects)
    {
        var propName = item.Name;
        var value = item.Value;
        if (item.HasValues)
        {
            var type = value.type;
            var format = value.format;
            var @ref = value["$ref"];
            if (@ref is not null)
            {
                var refName = @ref.Value;
                var innerObject = definitionsObjects[refName];
                ((IDictionary<string, object>)buildObject).Add(propName, innerObject);
            }
            else
            {
                var mockValue = BuildMockValue(type.ToString(), format?.ToString() ?? default);
                ((IDictionary<string, object>)buildObject).Add(propName, mockValue);
            }
        }
    }

    private IDictionary<string, dynamic> ParseDefinitions(IEnumerable<dynamic> definitions)
    {
        var _definitions = new Dictionary<string, dynamic>();
        foreach (var definition in definitions)
        {
            var properties = (IEnumerable<dynamic>)definition.Value.properties;
            dynamic definitionsObject = new ExpandoObject();
            foreach (var property in properties)
            {
                var propName = property.Name;
                var value = property.Value;
                if (property.HasValues)
                {
                    var propertyType = value.type;
                    var format = value.format;
                    var @ref = value["$ref"];
                    if (@ref is not null)
                        continue;
                    var mockValue = BuildMockValue(propertyType.ToString(), format?.ToString() ?? default);
                    ((IDictionary<string, object>)definitionsObject).Add(propName, mockValue);
                }
            }
            _definitions.Add($"#/definitions/{definition.Name}", definitionsObject);
        }
        return _definitions;
    }

    public object BuildMockValue(string? valueType, string? valueFormat)
    {
        switch (valueType)
        {
            case "string":
                {
                    if (!string.IsNullOrEmpty(valueFormat))
                    {
                        if (valueFormat == "date-time")
                        {
                            return _faker.Date.Soon();
                        }
                    }
                    return _faker.Random.String2(5);
                }
            case "integer":
                return _faker.Random.Int();
            case "number":
                return _faker.Random.Int(100);
            case "boolean":
                return _faker.Random.Bool();

            default: return default!;
        }
    }
}