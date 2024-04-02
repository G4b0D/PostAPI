# .NET API using Dapper and SQL Server

This project is a .NET API built using Dapper, a lightweight Object-Relational Mapping (ORM) library for .NET, and SQL Server as the backend database. The API provides endpoints to interact with the database, perform CRUD operations, and execute custom SQL queries efficiently.

## Features

- **Dapper**: Utilizes Dapper for data access to interact with the SQL Server database efficiently.
- **SQL Server**: Uses SQL Server as the backend database to store and manage data.
- **JWT Authentication**: Implements JWT (JSON Web Tokens) authentication for securing API endpoints.

## Getting Started

### Prerequisites

- .NET SDK installed on your machine.
- SQL Server installed and running.
- An understanding of JWT authentication concepts.

### Installation

1. Clone this repository to your local machine.
2. Run sql files under 'sql' directory in your db. Modify accordingly to your schema.
3. (Optional) Open SQLSeed project, configure connection string and run it using Visual studio or 'dotnet run'. This will populate the db with dummy data.
4. Configure your SQL Server connection string in the `appsettings.json` file.
5. Build the solution using Visual Studio or run `dotnet build` from the command line.
6. Run the API project. 

### Usage

1. **API Endpoints**: Use the provided API endpoints to interact with the database.
2. **JWT Authentication**: Authenticate users to access protected endpoints using JWT tokens. Ensure to include the JWT token in the Authorization header of your HTTP requests.
