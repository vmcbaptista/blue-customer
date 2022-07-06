# Blue Customer

## Introduction

This project implements a RESTful API for creating, updating, and retrieving customer information. A customer consists of the following fields:

- First name;
- Surname;
- E-mail address;
- Password.

The project was created with the Clean Architecture and Domain Driven Design in mind.

For such small project, we can argue that using such design is overkill. Although, I've done it so that I can demonstrate how this could be done in more domain rich projects.

With this said, the solution contains 3 projects

- Core - Contains Domain Entities (in this case, it's only Customer), Value Objects, handlers for CRUD operations and the definition of the interfaces used by Web API and Infrastructure.
- Infrastructure - Contains the implementation of the `ICustomerRepository` defined in the Core project. It is using Entity Framework Core 6 for that, which is abstracting the Microsoft SQL Server database. It includes also the database migrations code.
- Web API - Contains the actual RESTful API, which delegates all the processing to the handlers defined in Core

There are 3 tests project:

- Core - Unit tests some parts of the code under Core project. Particularly the handlers and a representation of some tests on Entities and Value Objects
- Web API - Unit tests the Controller
- Integration - Shows with a simple method the invocation of all the method exposed by the API

## Build and Run

To build and run, you can either open the solution inside Visual Studio or run it via docker compose by executing:
`docker-compose up`
This will startup an instance of MS SQL Server and the Web API. The database migrations run during Web API startup
When running with docker-compose Web API is exposed to port 8081 over HTTP and the MS SQL Server at port 4563. Credentials for MS SQL Server can be checked at docker-compose file.
The Web API can be tested by using the Swagger UI exposed via /swagger endpoint.

Another way to see the Web API in action is by running the Integration Tests, which could be done via `docker-compose -f docker-compose-integration.yml up --build --abort-on-container-exit`
This will behave like the first docker-compose command, but will also serve and run the Integration Tests

## Deployment considerations

How the API could:

1. Be deployed to a live environment;
   - Ideally, this would be deployed via an automated pipeline process. 
   - Let's say for example Azure DevOps pipeline, which would:
        1.  Build and prepare the container images
        2.  Deploy them to a container registry (for example Azure Container Registry) 
        3.  Update the container instance with the newly registered image (for example Azure App Services).
2. Handle a large volume of requests, including concurrent creation and update operations;
    - The safest way to ensure that there are no concurrency conflicts, is by using Pessimistic concurrency, which locks the database records that are being modified, preventing others to modify it a the same time. This could bring significant performance issues and therefore is not recommended to be used by web applications
    - So we shall focus on Optimistic Concurrency approach. With this approach, there is no lock involved, as it is assumed that the update in the majority of the cases will just work fine.When comes the moment where a conflict actually occurs, it shall then be handled, either by asking back the users to review the data to be updated, or by creating mechanisms that based on business requirements fix those conflicts in the best way possible.
    - To detect conflicts, we can either rely on an existing column, or create a new one specifically for that, the so called "RowVersion". The detection is done by using that column in the where condition that is appended to the original update query. In case that modified update query does not affect any row, it is an indicator that a conflict exists
    - In terms of creation, having incremental primary keys auto generated at the database level, or relying on unique identifiers as UUID/GUID shall avoid concurrency issues around the unique constraint of primary keys
3. Continue operating in the event of problems reading and writing from the database;
    - The introduction of a caching system, would allow that our system remains resilient to database failures as our application would target first the cache which could persist the data while the database is recovering
4. Ensure the security of the user information.
    - All the PII data in database shall be stored in an encrypted form.
    - Access to databases containing PII shall be restricted to a limited number of individuals
    - Implement mechanisms that purge PII that's not relevant anymore
