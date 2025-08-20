#### DANY OLIVEIRA - FUJITSU CHALLENGE

#### 14:00 - INICIAL COMMIT
* I chose .NET Core 8 for the backend, based on my previous experience and because it is very well suited for building APIs like the one requested.
* The project follows a layered architecture (Controllers -> Services -> Repositories), inspired by Clean Architecture.
	* Controllers: handle HTTP requests and responses.
	* Services: contain the business logic.
	* Repositories: manage persistence in JSON files.
* Data is stored in .json files to simplify the local setup and avoid external dependencies. This makes the project easy to run and test in any environment.

### 16:00 - AUTH SERVICE COMMIT
* STORAGE - Users are stored in a local users.json. This decision was made to avoid external dependencies (like SQL Server or EF Core) and keep the setup simple for local testing.
* PASSWORD HASHING - Passwords are stored as SHA256 hashes (Base64 encoded). This approach is sufficient for the scope of the challenge.
* JWT AUTHENTICATION - Each token includes the following claims:
   * id: the user’s unique identifier.
   * name: the username.
Tokens expire after the configured time (5 minutes).
JWT configuration (Key, Issuer, Audience, ExpireMinutes) is in appsettings.json.
* OUTPUT - A generic Response<T> was done to standardize success/failure responses across the application.
* DI - AuthService is registered as Scoped. This ensures a new instance per request while keeping file access consistent through the FileRepository.

### 17:45 - BOOK SERVICE COMMIT 
STORAGE – Books are stored in books.json.
CONTROLLER – Provides endpoints to create, list, update, and delete books. All endpoints are protected with [Authorize], ensuring only authenticated users can access them.

### 20:15 - FRONTEND COMMIT
HTML/CSS/JS - The main reason for this choice is that these are the frontend technologies I feel most comfortable with, which allowed me to implement the requested features more quickly within the challenge’s time limit.
To speed up development and improve the UI, I reused parts of templates (HTML/CSS snippets) as a base and adapted them to fit this project.

### 21:30 - FINAL COMMIT
USER ID - Just show their own books.
SETTINGS - Use of environment variables/configuration
UNIT TEST - I did test in AuthService and BookService

### POSSIBLE IMPROVEMENTS 
Replace file storage with a database (SQL Server, SQLite, or MongoDB).
Add password salt + stronger hashing.
More detailed unit/integration tests.
Improve UI/UX with a framework like Angular/React.

### HOW TO RUN? 
1- Build the project - dotnet build or directly in VS
2- Start the project - dotnet run --project Fujitsu.Challenge.API.csproj or directly in VS
3- Open the frontend in your browser at: https://localhost:7004/index.html
4- You can test the API directly using Swagger UI at: https://localhost:7004/swagger