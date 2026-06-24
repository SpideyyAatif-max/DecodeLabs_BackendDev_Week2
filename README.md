# UserVaultApi

A simple ASP.NET Core 8 Web API for managing users (full CRUD), built so you can run it locally and test every endpoint in Postman. Data is stored in memory (no database setup needed) and resets each time you restart the app.

## Folder contents

```
UserVaultApi/
├── Controllers/
│   └── UsersController.cs       # GET, POST, PUT, DELETE endpoints
├── Data/
│   └── UserStore.cs              # thread-safe in-memory "database"
├── Models/
│   └── User.cs                   # User entity + Create/Update DTOs
├── Properties/
│   └── launchSettings.json       # run profile, fixed to http://localhost:5236
├── appsettings.json
├── appsettings.Development.json
├── Program.cs                    # app startup, Swagger, CORS, DI
├── UserVaultApi.csproj
└── UserVaultApi.postman_collection.json   # import this straight into Postman
```

## Prerequisites

- .NET 8 SDK installed → https://dotnet.microsoft.com/download
- Postman (or use the built-in Swagger UI instead/as well)

## Run it

```bash
cd UserVaultApi
dotnet restore
dotnet run
```

You should see output confirming it's listening on `http://localhost:5236`. Leave this running in a terminal.

Optional: open `http://localhost:5236/swagger` in a browser to see/try the endpoints visually.

## Test in Postman

1. Open Postman → **Import** → select `UserVaultApi.postman_collection.json` from this folder.
2. The collection already has 5 requests set up, pointed at `http://localhost:5236/api/users` via the `{{baseUrl}}` variable.
3. With the app running (`dotnet run`), just hit **Send** on each request:

| Request | Method | URL | Notes |
|---|---|---|---|
| Get All Users | GET | `/api/users` | Returns the 2 seeded users initially |
| Get User By Id | GET | `/api/users/1` | Change the `userId` collection variable to test other ids |
| Create User | POST | `/api/users` | Body: `{ "name", "email", "password" }` |
| Update User | PUT | `/api/users/1` | Body: `{ "name", "email", "password" }` |
| Delete User | DELETE | `/api/users/1` | Returns `204 No Content` on success |

### Manual example (if you'd rather not import the collection)

**Create a user**
```
POST http://localhost:5236/api/users
Content-Type: application/json

{
  "name": "Grace Hopper",
  "email": "grace@example.com",
  "password": "compiler99"
}
```

**Get all users**
```
GET http://localhost:5236/api/users
```

**Get one user**
```
GET http://localhost:5236/api/users/1
```

**Update a user**
```
PUT http://localhost:5236/api/users/1
Content-Type: application/json

{
  "name": "Grace Hopper Updated",
  "email": "grace.updated@example.com",
  "password": "newpassword"
}
```

**Delete a user**
```
DELETE http://localhost:5236/api/users/1
```

## Notes / next steps

- Data lives in memory (`Data/UserStore.cs`) and resets on every app restart — swap this out for EF Core + SQL Server/SQLite/PostgreSQL if you want real persistence.
- Passwords are stored as plain text here for simplicity — never do this in a real/production app; hash them (e.g. with `BCrypt.Net` or ASP.NET Core Identity) before storing.
- CORS is wide open (`AllowAll`) for easy local testing — restrict this before deploying anywhere public.
