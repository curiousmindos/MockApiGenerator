using FluentAssertions;
using MockApi.ContentGenerator;

namespace MockApi.UnitTests;

public class CodeGeneratorUnitTests
{
    [Fact]
    public void Generate_JsonSchemaWithFlat_CreateCSharpClassesStringBuilder()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestMessage"",
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
                      ""type"": ""string""
                    },
                    ""optInFlag"": {
                      ""type"": ""boolean""
                    }
                  }
                }";

        // Act
        CSharpClassGenerator cSharpClassGenerator = new();
        var result = cSharpClassGenerator.CodeGenerate(jsonString).ToString();

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain("public class GuestMessage");
        result.Should().Contain("public string? FirstName");
        result.Should().Contain("public DateTime BirthDate");
    }

    [Fact]
    public void Generate_JsonSchemaWithReferenceObject_CreateCSharpClassesStringBuilder()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestMessage"",
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
                        },
                        ""Index"": {
                          ""type"": ""integer""
                        },
                      }
                    }
                  }
                }";

        // Act
        CSharpClassGenerator cSharpClassGenerator = new();
        var result = cSharpClassGenerator.CodeGenerate(jsonString).ToString();

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain("public class GuestMessage");
        result.Should().Contain("public string? FirstName");
        result.Should().Contain("public Inner Inner");
    }

    [Fact]
    public void Generate_JsonSchemaWithCollectionAndReferenceObject_CreateCSharpClassesStringBuilder()
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
                    ""inners"": {
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
        CSharpClassGenerator cSharpClassGenerator = new();
        var result = cSharpClassGenerator.CodeGenerate(jsonString).ToString();

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain("public class GuestMessage");
        result.Should().Contain("public string? FirstName");
        result.Should().Contain("public Inner Inner");
        result.Should().Contain("public ICollection<Inner> Inners");
    }

    [Fact]
    public void Generate_JsonSchemaWithNestedReferenceObject_CreateCSharpClassesStringBuilder()
    {
        // Arrange
        JsonSchemaBuilder cg = new JsonSchemaBuilder();
        var jsonString = @"{
                  ""$schema"": ""http://json-schema.org/draft-04/schema#"",
                  ""title"": ""GuestMessage"",
                  ""type"": ""object"",
                  ""additionalProperties"": false,                  
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
                    ""inners"": {
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
                        },
                        ""Product"": {
                          ""$ref"": ""#/definitions/Product""
                        },
                      }
                    },
                  ""Product"": {
                    ""type"": ""object"",
                    ""additionalProperties"": false,
                    ""properties"": {
                        ""id"": {
                          ""type"": [
                            ""null"",
                            ""string""
                          ]
                        },
                        ""code"": {
                          ""type"": ""string""
                        },
                        ""category"": {
                          ""type"": ""string""
                        }
                    }
                   }
                  }
                }";

        // Act
        CSharpClassGenerator cSharpClassGenerator = new();
        var result = cSharpClassGenerator.CodeGenerate(jsonString).ToString();

        // Assert
        result.Should().NotBeNull();
        result.Should().Contain("public class GuestMessage");
        result.Should().Contain("public string? FirstName");
        result.Should().Contain("public class Inner");
        result.Should().Contain("public class Product");
        result.Should().Contain("public Product Product");
        result.Should().Contain("public ICollection<Inner> Inners");
    }
}
