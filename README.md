# Mock Api WebApplication

This is an ASP.Net Core solution with API controller endpoints. 

The goal is to use this web application to register new mock API endpoints with settings:
- Text Response
- JSON Schema as HTTP Content Response
- URL path
- HTTP Method
- HTTP Response Code API call latency (optional)
- Validate Auhtorization header (optional)
- Validate JWT Autority token (optional)

You can use this for your tests on the client side or for integrating with other APIs. 
You can define API endpoints with expected behavior, response status code, and expected schema, API latency, etc.

1. The sample to register Mock API Response with single text response with Code 200 and latency 300 milliseconds:
```
{
  "httpMethod": "GET",
  "urlEndpointPath": "/my-customer-url-v2",
  "responseMessage": "Single Response with This URL Request",
  "httpStatusCode": 200,
  "latencyInMilliseconds": 300
}
```
2. with Authorization validation
```
{
  "httpMethod": "GET",
  "urlEndpointPath": "/my-customer-url-v2-auth-validate",
  "responseMessage": "Single Response with This URL Request with Beare Auth Header",
  "httpStatusCode": 300,
  "isAuthorizationValidate": true,
  "authority": "my-auth-server-authority-address"
}
```
3. with properties in JsonSchema
```
{
  "httpMethod": "GET",
  "urlEndpointPath": "/my-customer-url-v2",
  "httpStatusCode": 220,
  "latencyInMilliseconds": 3,
  "isAuthorizationValidate": true,
  "jsonSchema": {
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "GuestMessage",
    "type": "object",
    "additionalProperties": false,
    "properties": {
      "firstName": {
        "type": "string"
      },
      "lastName": {
        "type": "string"
      },
      "birthDate": {
        "type": "string",
        "format": "date-time"
      },
      "optInFlag": {
        "type": "boolean"
      }
    }
  }
}
```
4. with properties and inner objects in JsonSchema
```
{
  "httpMethod": "GET",
  "urlEndpointPath": "/my-customer-url-v2",
  "httpStatusCode": 204,
  "jsonSchema": {
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "GuestMessage",
    "type": "object",
    "additionalProperties": false,
    "properties": {
      "firstName": {
        "type": "string"
      },
      "lastName": {
        "type": "string"
      },
      "inner": {
        "$ref": "#/definitions/Inner"
      }
    },
    "definitions": {
      "Inner": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "Id": {
            "type": "number"
          }
        }
      }
    }
  }
}
```
5. with properies and inner object collection in JsonSchema
```
{
  "httpMethod": "GET",
  "urlEndpointPath": "/my-customer-url-v2",
  "httpStatusCode": 204,
  "jsonSchema": {
    "$schema": "http://json-schema.org/draft-04/schema#",
    "title": "GuestMessage",
    "type": "object",
    "additionalProperties": false,
    "properties": {
      "firstName": {
        "type": "string"
      },
      "lastName": {
        "type": "string"
      },
      "inner": {
        "type": "array",
        "items": {
          "$ref": "#/definitions/Inner"
        }
      }
    },
    "definitions": {
      "Inner": {
        "type": "object",
        "additionalProperties": false,
        "properties": {
          "Id": {
            "type": "number"
          }
        }
      }
    }
  }
}
```
Sample Response for Sample 5
```
{
  "firstName": "fuacq",
  "lastName": "tpdqi",
  "birthDate": "2024-05-06T17:48:52.9297819-04:00",
  "modifiedDate": "2024-05-07T09:31:36.0443116-04:00",
  "identifierNumber": 1273046372,
  "bookingId": 1574661347,
  "bolleanFlag": false,
  "inner": [
    {
      "Id": 319,
      "Name": "pkdvd",
      "Type": "tpdqiert"
    },
    {
      "Id": 368,
      "Name": "tpdqitet",
      "Type": "uuqug"
    }
  ]
}
```


 

