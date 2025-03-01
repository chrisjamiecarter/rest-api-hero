# REST API Hero

REpresentational State Transfer (REST) in an architectural style for building distributed hypermedia systems.

## Intoduction into RESTful APIs

### Constraints

- Uniform Interface
	- Identification of resources
	- Manipulation of resources through representations
	- Self-descriptive messages
	- Hypermedia as the engine of application state (HATEOAS)
- Stateless
- Cacheable
- Client-Server
- Layered System
- Code on Demand (optional)

### Resource Naming and Routing

- GET /movies (plural)
- GET /movies/1
- GET /movies/1/ratings
- GET /ratings/me
- PUT /movies/1/ratings
- DELETE /movies/1/ratings

### HTTP Verbs

- POST : Create
- GET : Read (Safe - does not change state)
- PUT : Complete Update
- PATCH : Partial Update
- DELETE : Delete

### HTTP Status Codes

POST
- Single resource (/items/1): N/A
- Collection resource (/items): 201 (Location header), 202

GET
- Single resource (/items/1): 200, 404
- Collection resource (/items): 200

PUT
- Single resource (/items/1): 200, 204, 404
- Collection resource (/items): 405

DELETE
- Single resource (/items/1): 200, 404
- Collection resource (/items): 405

### REST API Response Body

JSON
```
{
	"key": "value"
}
```

XML
```
<key>value</key>
```

### Idempotency

No matter how many times you apply the operation, the result will always be the same.

POST - NOT Idempotent
GET - Idempotent
PUT - Idempotent
DELETE - Idempotent
HEAD - Idempotent
OPTIONS - Idempotent
TRACE - Idempotent

### Hypermedia as the Engine of Application State (HATEOAS)

```
{
	"deparmentId": 10,
	"departmentName": "Administraion",
	"locationId": 1700,
	"managerId": 200
	"links": [
		{
			"href": "10/employees",
			"rel": "employees",
			"type": "GET"
		}
	]
}
```

```
{
	"account": {
		"account_number": 12345,
		"balance": {
			"currency": "usd",
			"value": 100.00
		}
		"links": {
			"deposits": "/accounts/12345/deposits",
			"withdrawals": "/accounts/12345/withdrawals",
			"transfers": "/accounts/12345/transfers",
			"close-requests": "/accounts/12345/close-requests"
		}
	}
}
```

### Errors

Error - Client sends invalid data, returns 400.
Fault - Server encounters an error, returns 500.
