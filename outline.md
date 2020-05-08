# Application Development Flow Outline

This is the development flow that we follow to build the contacts application

## Introduction to the IDE

Do we want to start with introducing Visual Studio? Visual Studio Code? JetBrains Rider?

* Video: [Introduction to IDEs - Part 1](https://youtu.be/19nRZ6CBXDI): [Microsoft Visual Studio](https://visualstudio.microsoft.com/), [Microsoft Visual Studio Code](https://code.visualstudio.com/?wt.mc_id=DX_841432).
* Video: [Introduction to IDEs - Part 2](https://youtu.be/qlg9n-T064Q): [JetBrains](https://www.jetbrains.com/) [Rider](https://www.jetbrains.com/rider/)

## Domain

### Model Development

Built out the Contacts.Domain models.  [Video](https://youtu.be/XkCFW9rqSz0) Commit Id: [0e3443be](https://github.com/jguadagno/Contacts/commit/0e3443bfe8657f250fd0830e0e36ae22a8ec3a62)

### Business / Manager Layer

Built out the Manager Layer. [Video](https://youtu.be/wZmzM3AWAyk) Commit Id: [f89e811](https://github.com/jguadagno/Contacts/commit/f89e8116ae3bbefe20f50b6a8fdfcd642e34db38)

### Data Layer

Let's persist the data.

Adding the Sqlite Database, EntityFrameworkCore, and EntityFramework migrations

([Video](https://youtu.be/kf3hQ1rt8SY)) Commit Id: [ef63479](https://github.com/jguadagno/Contacts/commit/ef63479328252ce06c6312aa62446f294deb7e58)

#### Helpful Links

* [Getting Started with EF Core](https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=netcore-cli)
* [Managing Connection Strings in Entity Framework Core](https://www.learnentityframeworkcore.com/connection-strings)
* [SQLite Connection Strings](https://www.connectionstrings.com/sqlite/)
* [SQLite](https://sqlite.org/index.html)

### Unit Test the Application

Introduce [Moqs](https://github.com/Moq/moq4) and/or [Microsoft Fakes](https://docs.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes?view=vs-2019) or [xUnit](https://xunit.net/)

## Data Persistance Layer

### Data Objects

### Data Mapping

Map from Domain classes to the data storage

Add unit tests to validate mapping?

### Data Manager

Commonly referred to as a Repository or follow the repository pattern

## Expose the Application

Let's take the applications *Business Layer* and expose it via an API

### New Web API

### Validate the API Works

Use Postman and/or HttpRequest in Visual Studio Code and/or Rider's HttpClient

## To the Cloud

Setup the environment

**NOTE**: These are tentative and might change as we build out the application

* Azure SQL Database ([docs](https://azure.microsoft.com/en-us/services/sql-database/))
* Azure Web Apps ([docs](https://azure.microsoft.com/en-us/services/app-service/web/))
* Azure API Management ([docs](https://azure.microsoft.com/en-us/services/api-management/)
* Blog Storage ([docs](https://azure.microsoft.com/en-us/services/storage/blobs/))
* Azure CDN ([docs](https://azure.microsoft.com/en-us/services/cdn/))
* Azure Active Directory ([docs](https://azure.microsoft.com/en-us/services/active-directory/))
* Azure Key Vault ([docs](https://azure.microsoft.com/en-us/services/key-vault/))

*Maybe* use Terraform to build out the environment

### Publish the API

> Friends don't let friends right click and publish

On commit to the source code repository...

* Build the site using Azure Pipelines or Github Actions
* Execute the Unit Tests
* Deploy the API to the development slot of the Azure Web App
* Once successful, 'click to deploy' to production

**NOTE** Figure out database changes

### API Authentication

Add client id / client secret to API

## Client Applications

Add a picture for the person.

Uses:

* Blog Storage
* Queues
* CDN to host the images

Update the models, data store, and API

### ASP.NET Core Web Application

### Blazor Client

### Native Application

#### React Native

#### Ionic client

### WinForms (Maybe)

## Application Flexibility

Let's see how the application changes as we change different components of it.

### Data Persistance

Let's change out the database

#### Move to Json file

#### Move to AWS Dynamo
