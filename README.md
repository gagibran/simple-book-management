[![Build And Test](https://github.com/gagibran/book-rental-manager/actions/workflows/build-and-test.yml/badge.svg)](https://github.com/gagibran/book-rental-manager/actions/workflows/build-and-test.yml)

# Book Rental Manager

## Description
The backend of a fake book rental management system created with .NET, PostgreSQL, Swagger, clean architecture with CQRS and some domain-driven design techniques. I am also using HATEOAS in order to improve API discoverability. The goal of this project is to practice these technologies and techniques. I have not implemented authentication nor the frontend for this project.

## Requirements
* .NET 8.0.x;
* PostgreSQL 16.0.

## Creating an admin user on PostgreSQL
This is required in order to run the application. After installing PostgreSQL on your machine:
* Windows users:
    1. Put the directory `C:\Program Files\PostgreSQL\15\bin` in the `PATH` variable;
    2. Run `psql -U postgres` and type in the password set up during the PostgreSQL installation;
    3. Run `CREATE ROLE admin WITH CREATEDB LOGIN PASSWORD 'admin';`.
* Linux users:
    1. Run `sudo su - postgres`;
    2. Run `psql`;
    3. Run `CREATE ROLE admin WITH CREATEDB LOGIN PASSWORD 'admin';`.

## Running the application
Docker Compose; Needs to have .pfx cert. Fix this later.

To generate the required PFX certificate, go to the project's root folder and execute the following commands:
1. `dotnet dev-certs https -ep httpsdevcert.pfx -p HttpsDevCertPass1!`
2. `dotnet dev-certs https --trust`.

`dotnet watch run` will open up the application with Swagger on https://localhost:5001/swagger/index.html. Starting the application for the first time will also create a database called `BookRentalManager` (in case it does not exist) with some seed data in its tables. For more information on the seed data, check the file `src/BookRentalManager.Infrastructure/Data/Seeds/TestDataSeeder.cs`.

## Migrations
In case someone wants to fork or expand on this project, this is how I set up migrations. We'll need [Entity Framework Core tools](https://learn.microsoft.com/en-us/ef/core/cli/dotnet) installed for this.

### Create a migration
`dotnet ef migrations add <MigrationName> -s .\src\BookRentalManager.Api\ -p .\src\BookRentalManager.Infrastructure\ -o Data\Migrations\`.

### Remove a migrations
`dotnet ef migrations remove -s .\src\BookRentalManager.Api\ -p .\src\BookRentalManager.Infrastructure\`.

### Update the database after creating or removing a migration
`dotnet ef database update -s .\src\BookRentalManager.Api\ -p .\src\BookRentalManager.Infrastructure\`.