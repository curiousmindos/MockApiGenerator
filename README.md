# Mock Api WebApplication

This is an ASP.Net Core solution with API controller endpoints. 

The goal is to use this web application to register new mock API endpoints with settings:
- JSON Schema as HTTP Content Response
- URL path
- HTTP Method
- HTTP Response Code API call latency

You can use this for your tests on the client side or for integrating with other APIs. 
You can define API endpoints with expected behavior, response status code, and expected schema, API latency, etc.

Here's an example of what the API registry payload looks like for creating a new API URL endpoint with a GET and 230 code response and a generated mock JSON response based on the input JSON schema:

```{
  "httpMethod": "GET",
  "urlEndpointPath": "/my-customer-url-v2",
  "httpStatusCode": 220,
  "latencyInSec": 3,
  "jsonSchema": {
  "$schema": "http://json-schema.org/draft-04/schema#",
  "title": "GuestEnrollmentMessage",
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
    "identifierNumber": {
      "type": "string"
    },
	"bookingId": {
      "type": "number"
    },
    "optInFlag": {
      "type": "boolean"
    },
	"myStringArray": {
	  "type": "array",
	  "items": {
		"type": "string"
	  }
	},
	"myArray": {
	  "type": "array",
	  "items": {
		"type": "number"
	  }
	}
  }
 }
}
```
Screenshots:

1. POST Payload In Swagger:
   
![image](https://github.com/curiousmindos/MockApiGenerator/assets/7238801/7a9e9420-638b-457b-8f46-9d0df13f1723)

3. Sample how to use registered mock API endpoint
   
![image](https://github.com/curiousmindos/MockApiGenerator/assets/7238801/eb8ac6cc-0348-4e36-bfe3-31b040c613e8)

 

