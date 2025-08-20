#### DANY OLIVEIRA - FUJITSU CHALLENGE

#### 14:00 - INICIAL COMMIT
* I chose .NET Core 8 for the backend, based on my previous experience and because it is very well suited for building APIs like the one requested.
* The project follows a layered architecture (Controllers → Services → Repositories), inspired by Clean Architecture.
	* Controllers: handle HTTP requests and responses.
	* Services: contain the business logic.
	* Repositories: manage persistence in JSON files.
* Data is stored in .json files to simplify the local setup and avoid external dependencies. This makes the project easy to run and test in any environment.