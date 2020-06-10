# Application Development Flow Outline

This is the development flow that we follow to build the contacts application

## Introduction to the IDE

Do we want to start with introducing Visual Studio? Visual Studio Code? JetBrains Rider?

* Video: [Introduction to IDEs - Part 1](https://youtu.be/19nRZ6CBXDI): [Microsoft Visual Studio](https://visualstudio.microsoft.com/), [Microsoft Visual Studio Code](https://code.visualstudio.com/?wt.mc_id=DX_841432).
* Video: [Introduction to IDEs - Part 2](https://youtu.be/qlg9n-T064Q): [JetBrains](https://www.jetbrains.com/) [Rider](https://www.jetbrains.com/rider/)

## Domain

### Model Development

Built out the Contacts.Domain models.  

* [Video](https://youtu.be/XkCFW9rqSz0)
* Source Code [0e3443be](https://github.com/jguadagno/Contacts/commit/0e3443bfe8657f250fd0830e0e36ae22a8ec3a62)

### Business / Manager Layer

Built out the Manager Layer.

* [Video](https://youtu.be/wZmzM3AWAyk)
* Source Code [f89e811](https://github.com/jguadagno/Contacts/commit/f89e8116ae3bbefe20f50b6a8fdfcd642e34db38)

### Data Layer

Let's persist the data.

**Part 1**: Adding the Sqlite Database, EntityFrameworkCore, and EntityFramework migrations

* [Video](https://youtu.be/kf3hQ1rt8SY)
* Source Code [ef63479](https://github.com/jguadagno/Contacts/commit/ef63479328252ce06c6312aa62446f294deb7e58)

**Part 2**: Connecting the Contacts Manager to the Database

* [Video](https://youtu.be/CSTHbmINagM)
* Source Code [242a970](https://github.com/jguadagno/Contacts/commit/242a97023bee18972325e0c253d9339c056edaca)

#### Helpful Links

Reference guides/posts that were used to build out the data layer

* [Getting Started with EF Core](https://docs.microsoft.com/en-us/ef/core/get-started/?tabs=netcore-cli)
* [Managing Connection Strings in Entity Framework Core](https://www.learnentityframeworkcore.com/connection-strings)
* [SQLite Connection Strings](https://www.connectionstrings.com/sqlite/)
* [SQLite](https://sqlite.org/index.html)

### Unit Test the Application

Introduced Unit Testing to the Application with [xUnit](https://xunit.net/)

* [Video](https://youtu.be/azlDSfepbEo)
* Source Code [3549059](https://github.com/jguadagno/Contacts/commit/35490594fa1520128b1965cb7ff743a56fdef8ce)

Introduced the *poor persons* dependency injection

* [Video](https://youtu.be/VkAZmWauQeA)
* Source Code [7a71d94](https://github.com/jguadagno/Contacts/commit/0b7a94ec6fb42c57899e85e20c44d83004bd347c)

Introduced [Moqs](https://github.com/Moq/moq4) to mock out unit tests and avoid using the database with running units tests.

* [Video](https://youtu.be/Yy6LK9k9ZS8)
* Source Code [0421077](https://github.com/jguadagno/Contacts/commit/042107737c3b403e18b4d430bd8f1ef69ba9664e)

As an alternative to Moqs, you can look at
[Microsoft Fakes](https://docs.microsoft.com/en-us/visualstudio/test/isolating-code-under-test-with-microsoft-fakes?view=vs-2019) or Telerik [Just Mocks](https://www.telerik.com/products/mocking.aspx)

Check out the code coverage for our unit tests and ended up adding more tests.

* [Video](https://youtu.be/Ccyf3FyKZDU)
* Source Code [e1c0985](https://github.com/jguadagno/Contacts/commit/e1c09853b0335fa8c4f94e732508208c6e8f0c92)

## Data Persistence Layer

### Data Objects

We build out the new Sqlite.Models in preparation of changing databases.  

### Data Mapping

Map from Domain classes to the data storage. We also implemented [Automapper](https://www.automapper.org) to the project.

* [Video](https://youtu.be/MKeGYHjfhf8)
* Source Code [00dfc82](https://github.com/jguadagno/Contacts/commit/00dfc825f4fe69c3f1c7033bed1d685d42319513)

## Expose the Application

Let's take the applications *Business Layer* and expose it via an API

The new API is based off of [Create a web API](https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api)

### New Web API

First round, we added the GetContacts, GetContact, and SaveContact

* [Video](https://youtu.be/A0I6iCbyc1Q)
* Source Code [d309dfe](https://github.com/jguadagno/Contacts/commit/d309dfe3e6f4b29cd3c472f6ba3632ae17fbf61a)

Second round, we added the DeleteContact and GetContacts (Search) endpoints. Improved the SaveContact endpoint.

Next up:

* [Video](https://youtu.be/ee247e0CH6k)
* Source Code [e77b039](https://github.com/jguadagno/Contacts/commit/e77b039fe1d7b4eb7bd3805d93aff2862112bfbf)

- [ ] 
### Validate the API Works

Use Postman and/or HttpRequest in Visual Studio Code and/or Rider's HttpClient

### Document the Api

Let's us [Swagger](https://swagger.io/) to add documentation to our Api

The [example](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio) we followed along with on the stream/video

* [Video](https://youtu.be/7xVmrorkH2U)
* Source Code [397d781](https://github.com/jguadagno/Contacts/commit/c7fa40c623af0b1acfd5ae3a3d7427e1922c8d45)

### Improve the Api

We validated that the Api allows for saving the reference properties or address and phone.

We used ASP.NET Core Dependency Inject to create the `ContactManager` and required dependencies

* [Video](https://t.co/o8Y3O8ighf)
* Source Code [3f55ac17](https://github.com/jguadagno/Contacts/commit/3f55ac17654a23a2156b4b831bf4f45706142cad)

Worked on getting the first part of the `phones` endpoint.

* [Video](https://youtu.be/g-0x-JjR0_U)
* Source Code [82690ea5](https://github.com/jguadagno/Contacts/commit/82690ea539c25df74c00bc52ca6d07865f7e9b24)

## To the Cloud

Setup the environment

**NOTE**: These are tentative and might change as we build out the application

* Azurite ([docs](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite))
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
