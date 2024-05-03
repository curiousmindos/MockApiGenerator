using Bogus;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using Namotion.Reflection;

namespace MockContentGenerator;

/// <summary>
/// ContentGenerator.
/// </summary>
public class ContentGenerator
{
    private readonly Faker _faker;
    
    public ContentGenerator()
    {
        _faker = new Faker();
    }

    public string GenerateBySchema(JsonDocument apiValuedRoute)
    {
        JObject newObject = new JObject();

        foreach (var jsonProperty in apiValuedRoute.RootElement.GetProperty("properties").EnumerateObject())
        {
            //todo with "$ref"
            string? valueType = jsonProperty.Value.GetProperty("type").ToString();
            string? valueFormat = jsonProperty.Value.HasProperty("format") ? jsonProperty.Value.GetProperty("format").ToString() : default;

            if (valueType == "array")
            {
                var items = jsonProperty.Value.GetProperty("items");
                var arrayType = items.GetProperty("type").ToString();
                var array = new JArray();
                var itemCount = _faker.Random.Int(1, 3);
                for (var i = 0; i < itemCount; i++)
                {
                    array.Add(new JValue(BuildMockValue(arrayType, null)));
                }

                newObject.Add(new JProperty(jsonProperty.Name, array));
            }
            else
            {
                newObject.Add(new JProperty(jsonProperty.Name, BuildMockValue(valueType, valueFormat)));
            }
        }
        return newObject.ToString();
    }

    private object BuildMockValue(string? valueType, string? valueFormat)
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
