using FluentAssertions;
using MockApi.ContentGenerator;
using Newtonsoft.Json;

namespace MockApi.UnitTests;

public class JsonSchemaParserUnitTests
{

    [Fact]
    public void Parse_JsonSchema_CreateObjectWithProperties()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestEnrollmentMessage"",
                  ""type"": ""object"",
                  ""additionalProperties"": false,
                  ""required"": [
                    ""firstName"",
                    ""lastName"",
                    ""birthDate"",
                    ""identifierNumber"",
                    ""optInFlag""
                  ],
                  ""properties"": {
                    ""firstName"": {
                      ""type"": ""string""
                    },
                    ""lastName"": {
                      ""type"": ""string""
                    },
                    ""birthDate"": {
                      ""type"": ""string"",
                      ""format"": ""date-time""
                    },
                    ""identifierNumber"": {
                      ""type"": ""string""
                    },
                    ""optInFlag"": {
                      ""type"": ""boolean""
                    }
                  }
                }";

        // Act
        var jsonResult = cg.Build(jsonString);
        dynamic dynamicResult = JsonConvert.DeserializeObject(jsonResult.jsonSerializedContent)!;            
        
        // Assert
        jsonResult.Should().NotBeNull();
        ((object)dynamicResult.birthDate).Should().NotBeNull();
        ((string)dynamicResult.firstName).Should().NotBeNull();
        
        bool birthDateString = DateTime.TryParse(dynamicResult.birthDate.ToString(), out DateTime resultDateTime);
        birthDateString.Should().BeTrue();
    }

    [Fact]
    public void Parse_JsonSchema_CreateObjectWithNullableProperties()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestEnrollmentMessage"",
                  ""type"": ""object"",
                  ""additionalProperties"": false,
                  ""required"": [
                    ""firstName"",
                    ""lastName"",
                    ""birthDate"",
                    ""identifierNumber"",
                    ""optInFlag""
                  ],
                  ""properties"": {
                    ""firstName"": {
                      ""type"": [
                        ""null"",
                        ""string""
                      ]
                    },
                    ""lastName"": {
                      ""type"": ""string""
                    },
                    ""birthDate"": {
                      ""type"": ""string"",
                      ""format"": ""date-time""
                    },
                    ""identifierNumber"": {
                      ""type"": [
                        ""integer"",
                        ""null""
                      ],
                      ""format"": ""int32""
                    },
                    ""optInFlag"": {
                      ""type"": ""boolean""
                    }
                  }
                }";

        // Act
        var jsonResult = cg.Build(jsonString);
        dynamic dynamicResult = JsonConvert.DeserializeObject(jsonResult.jsonSerializedContent)!;

        // Assert
        jsonResult.Should().NotBeNull();
        ((object)dynamicResult.birthDate).Should().NotBeNull();
        ((string)dynamicResult.firstName).Should().NotBeNull();

        bool birthDateString = DateTime.TryParse(dynamicResult.birthDate.ToString(), out DateTime resultDateTime);
        birthDateString.Should().BeTrue();
    }

    [Fact]
    public void Parse_JsonSchemaWithInnerObject_CreateObjectWithInnerObject()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestEnrollmentMessage"",
                  ""type"": ""object"",
                  ""additionalProperties"": false,
                  ""required"": [
                    ""firstName"",
                    ""lastName"",
                    ""birthDate"",
                    ""identifierNumber"",
                    ""optInFlag"",
                    ""inner""
                  ],
                  ""properties"": {
                    ""firstName"": {
                      ""type"": ""string""
                    },
                    ""lastName"": {
                      ""type"": ""string""
                    },
                    ""birthDate"": {
                      ""type"": ""string"",
                      ""format"": ""date-time""
                    },
                    ""identifierNumber"": {
                      ""type"": ""string""
                    },
                    ""optInFlag"": {
                      ""type"": ""boolean""
                    },
                    ""inner"": {
                      ""$ref"": ""#/definitions/Inner""
                    }
                  },
                  ""definitions"": {
                    ""Inner"": {
                      ""type"": ""object"",
                      ""additionalProperties"": false,
                      ""properties"": {
                        ""Id"": {
                          ""type"": ""string""
                        }
                      }
                    }
                  }
                }";            

        // Act
        var jsonResult = cg.Build(jsonString);
        dynamic dynamicResult = JsonConvert.DeserializeObject(jsonResult.jsonSerializedContent)!;

        // Assert
        jsonResult.Should().NotBeNull();
        ((object)dynamicResult.inner).Should().NotBeNull();
    }

    [Fact]
    public void Parse_JsonSchemaWithRefArray_CreateObjectWithCollection()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestEnrollmentMessage"",
                  ""type"": ""object"",
                  ""additionalProperties"": false,                  
                  ""properties"": {
                    ""firstName"": {
                      ""type"": ""string""
                    },
                    ""lastName"": {
                      ""type"": ""string""
                    },                    
                    ""inner"": {
                      ""type"": ""array"",
                      ""items"": {
                        ""$ref"": ""#/definitions/Inner""
                      }
                    }
                  },
                  ""definitions"": {
                    ""Inner"": {
                      ""type"": ""object"",
                      ""additionalProperties"": false,
                      ""properties"": {
                        ""Id"": {
                          ""type"": ""string""
                        }
                      }
                    }
                  }
                }";

        // Act
        var jsonResult = cg.Build(jsonString);
        dynamic dynamicResult = JsonConvert.DeserializeObject(jsonResult.jsonSerializedContent)!;

        // Assert
        jsonResult.Should().NotBeNull();
        ((object)dynamicResult.inner).Should().NotBeNull();
        ((object)dynamicResult.inner).Should().BeOfType<Newtonsoft.Json.Linq.JArray>();
        var collection = ((object)dynamicResult.inner) as Newtonsoft.Json.Linq.JArray;
        collection?.Count.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Parse_JsonSchemaWithNumberArray_CreateObjectWithCollection()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestEnrollmentMessage"",
                  ""type"": ""object"",
                  ""additionalProperties"": false,                  
                  ""properties"": {
                    ""firstName"": {
                      ""type"": ""string""
                    },
                    ""lastName"": {
                      ""type"": ""string""
                    },                    
                    ""numberArray"": {
                      ""type"": ""array"",
                      ""items"": {
                        ""type"": ""integer"",
                        ""format"": ""int32""
                      }
                    }
                  },
                  ""definitions"": {
                    ""Inner"": {
                      ""type"": ""object"",
                      ""additionalProperties"": false,
                      ""properties"": {
                        ""Id"": {
                          ""type"": ""string""
                        }
                      }
                    }
                  }
                }";

        // Act
        var jsonResult = cg.Build(jsonString);
        dynamic dynamicResult = JsonConvert.DeserializeObject(jsonResult.jsonSerializedContent)!;

        // Assert
        jsonResult.Should().NotBeNull();
        ((object)dynamicResult.numberArray).Should().NotBeNull();

        var collection = ((object)dynamicResult.numberArray) as Newtonsoft.Json.Linq.JArray;
        collection?.Count.Should().BeGreaterThan(0);
    }
}