CDRM .NET Core Assessment API
This is a sample ASP.NET Core Web API for storing and retrieving documents in different formats.

Features
-Store documents with id, tags, and any data (POST/PUT in JSON)
-Get documents in JSON, XML, or MessagePack (use the Accept header)
-Easy to add new formats or storage backends
-In-memory storage for fast reads (can be replaced with SQL, Redis, etc.)
-Model validation for required fields
-Unit tests for controllers, storage, and formatters

Endpoints
-POST /documents — Add a document
-PUT /documents/{id} — Update a document
-GET /documents/{id} — Get a document (set Accept to application/json, application/xml, or application/x-msgpack)

Notes
-All fields (id, tags, data) are required.
-Data is not saved after restart (in-memory only).
-See unit tests in the CDRM_NetCore_Assessment.Tests project.
