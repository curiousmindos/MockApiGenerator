# MockApiWebApplication

This is ASP.Net Core solution with API controller endpoints 

The target of this solution is to use this Web Application to register new Mock API endpoints with settings
- JsonSchema as Http Content Response
- Url path
- Http Method
- Http Response Code
- API call latency

  Sample of API registry Payload to create new API Url Endpoint with GET and 230 Code response and generated Mock Json Response based on input JsonSchema:
  {
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

Screen shots:
1. POST Payload In Swagger:
![image](https://github.com/curiousmindos/MockApiGenerator/assets/7238801/7a9e9420-638b-457b-8f46-9d0df13f1723)

2. Sample how to use registered mock API endpoint
![image](https://github.com/curiousmindos/MockApiGenerator/assets/7238801/eb8ac6cc-0348-4e36-bfe3-31b040c613e8)

 

