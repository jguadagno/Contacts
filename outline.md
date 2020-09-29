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

### Validate the API Works

Use Postman and/or HttpRequest in Visual Studio Code and/or Rider's HttpClient

### Document the Api

Let's us [Swagger](https://swagger.io/) to add documentation to our Api

The [example](https://docs.microsoft.com/en-us/aspnet/core/tutorials/getting-started-with-swashbuckle?view=aspnetcore-3.1&tabs=visual-studio) we followed along with on the stream/video

* [Video](https://youtu.be/7xVmrorkH2U)
* Source Code [397d781](https://github.com/jguadagno/Contacts/commit/c7fa40c623af0b1acfd5ae3a3d7427e1922c8d45)

### Improve the Api

We validated that the Api allows for saving the reference properties or address and phone.

We used ASP.NET Core Dependency Injection to create the `ContactManager` and required dependencies

* [Video](https://t.co/o8Y3O8ighf)
* Source Code [3f55ac17](https://github.com/jguadagno/Contacts/commit/3f55ac17654a23a2156b4b831bf4f45706142cad)

Worked on getting the first part of the `phones` endpoint.

* [Video](https://youtu.be/g-0x-JjR0_U)
* Source Code [82690ea5](https://github.com/jguadagno/Contacts/commit/82690ea539c25df74c00bc52ca6d07865f7e9b24)

Wrapped up the Api by adding methods, endpoints, tests, and documentation for the GetContactPhones, GetContactPhone, GetContactAddresses, and GetContactAddress

* [Video](https://youtu.be/eiauddTufQU)
* Source Code [abe7719](https://github.com/jguadagno/Contacts/commit/abe7719ad53b9bac8067f807ee125bdc9416828b)

### API Authentication

Add client id / client secret to API
Added Authentication and Authorization using the [Microsoft Identity Platform](https://docs.microsoft.com/en-us/azure/active-directory/develop/). 
This was part of a blog post [Protecting an ASP.NET Core Web API with Microsoft Identity Platform](https://www.josephguadagno.net/2020/06/12/protecting-an-asp-net-core-api-with-microsoft-identity-platform)

* [Video](https://youtu.be/8sCR0hLeMxM)
* Source [a611c6f](https://github.com/jguadagno/Contacts/commit/a611c6f9c8f027dbcfef365a2a5edf665636213a)

Finished the authentication permissions for the Contacts API

* [Video](https://youtu.be/31GJSN4D3d4)
* Source [90c79b6](https://github.com/jguadagno/Contacts/commit/90c79b66b6f7c5891d50aa3267d173483cf6689e)

Adding Asynchronous ability to our methods.

* [Video](https://youtu.be/XWjaXSVUJMQ)
* Source [748cf3e](https://github.com/jguadagno/Contacts/commit/748cf3edd654c52c637543939b72eac5954231b7)

## Let's Build Some Clients

### ASP.NET Core MVC

Authentication is based off of the following repository
[Enable your Web Apps to sign-in users and call APIs with the Microsoft identity platform for developers](https://github.com/Azure-Samples/active-directory-aspnetcore-webapp-openidconnect-v2)

Started off creating the New ASP.NET Core Web Application using Model View Controller (MVC). We added the authentication to the application. Then we changes some of the home page text.

* [Video](https://youtu.be/ffsR0ms2XoU)
* Source [46c8e58](https://github.com/jguadagno/Contacts/commit/46c8e58f17ccf91be2ab2ef6a512b4663f77b2c7)

Added the Contacts Controller with an Index method. **NOTE** The code compiles but the authorization is broken.

* [Video](https://youtu.be/NV1fa81bIpY)
* Source [d7a7111](https://github.com/jguadagno/Contacts/commit/d7a71110fb3b4baf20a87926465fa9753d64c3ff)

Successful authenticated against the API.
Send a request for Get Contacts
Displayed simple results on the web page

* [Video](https://youtu.be/GjSeJbZaPOw)
* Source [7eb353b](https://github.com/jguadagno/Contacts/commit/7eb353b13ca1a0b135e1a777c718f96bdb353025)

Special 'Cleanup' Episode!  

* We added a new Contact Service [Source](https://github.com/jguadagno/Contacts/commit/9f5c57778b65c5411c6224b839864829ff84879b)
* We moved the configuration of the SQLite Database to the AppSetting file [Source](https://github.com/jguadagno/Contacts/commit/043f9b5da795aa0d00f715c9394cd3e7d43ba6e0)
* Removed an unneeded TODO comment [Source](https://github.com/jguadagno/Contacts/commit/22d59eb5603de831c93f701e260c5faca08b70bf)

* [Video](https://youtu.be/U4-l0IyqA6k)
* Source [22d59eb](https://github.com/jguadagno/Contacts/commit/22d59eb5603de831c93f701e260c5faca08b70bf)

Built out the Contact List and Details Page

* [Video](https://youtu.be/hXcKGbvsaWM)
* Source [e6f70de](https://github.com/jguadagno/Contacts/commit/e6f70de7eba389321400dc674f8dca56993d96fc)

Built out the Contact Edit page.  I also fixed a bug that had to with using the CreatedAtAction in the Contacts.Api. Blog Post coming soon.

* [Video](https://youtu.be/OsNIjcnP40s)
* Source [ef1f842](https://github.com/jguadagno/Contacts/commit/ef1f84225c6c1e6e75ef0ee6c339b77d9039ebe8)

Built out the Contact Delete Page

Built out the Contact Add Page

* [Video](https://youtu.be/9YeOythkTlE)
* Source [e1f15c7](https://github.com/jguadagno/Contacts/commit/e1f15c79b795b0da2b447d60f7537878c5d5d604)

Cleaned up the User Interface by adding some [Icons](https://icons.getbootstrap.com/) from the [Bootstrap UI](https://www.getbootstrap.com/) library.

* [Video](https://youtu.be/N0Z96Al4ICk)
* Source [8701a32](https://github.com/jguadagno/Contacts/commit/8701a324a1605cb8f931b108960e7b8c2d784adf)

## Migrate to SQL Server

We start the database migration from SQLite to Microsoft SQL Server

Part I: Move create the database and copy the data

* [Video](https://youtu.be/ghN2dHanwCU)
* No code for this

Part II: Create the `Contacts.Data.SqlServer` data store

* [Video](https://youtu.be/VQXF1GmkvrY)
* Source [4c10c56](https://github.com/jguadagno/Contacts/commit/4c10c56875077097b20b604197b200c4cabf086b)

## Move to the Cloud

We move our application from our local machine to the Cloud

Part I: Create the database, app service plan, API app service, and UI App Service.  We then migrate the data off to the new SQL Server

* [Video](https://youtu.be/dsrH75nSXdo)
* *No Source Code*

Part II: Upload the API to the App Service and connect the database

* [Video](https://youtu.be/0b4cHz6p-FQ)
* Source [f79fd6d](https://github.com/jguadagno/Contacts/commit/f79fd6d6605e86e5a35c6be0250349b67f698085)

Part III: Upload the Web UI to the App Service and connect it to the API

* [Video](https://www.youtube.com/watch?v=4rLaB_uk7X8)
* Source [bb89dfca](https://github.com/jguadagno/Contacts/commit/bb89dfcad720b6036850452e9c8f7ad78f37e866)

## Application Insights

We added [Application Insights](https://docs.microsoft.com/en-us/azure/azure-monitor/app/app-insights-overview) to the Web UI and API.

* [Video](https://www.youtube.com/watch?v=w5DcUrhk5wU)
* Source [6338048](https://github.com/jguadagno/Contacts/commit/6338048e5a06eed0e47fa2a01d32f3e7794d7096)

## Application Logging

We added logging to the application using [Nlog](https://nlog-project.org)

Helpful links

* [Getting started with ASP.NET Core 3](https://github.com/NLog/NLog/wiki/Getting-started-with-ASP.NET-Core-3)
* List of [Targets](https://nlog-project.org/config/)
  * [Database](https://github.com/NLog/NLog/wiki/Database-target)
  * [Application Insights](https://github.com/Microsoft/ApplicationInsights-dotnet-logging)

* [Video](https://youtu.be/8zTFfII4PXg)
* Source Code [6bb62af](https://github.com/jguadagno/Contacts/commit/6bb62afd8926d6503fbcf7cccd319413cdf5eb2e)

## Publish on Commit

> Friends don't let friends right click and publish

On commit to the source code repository...

* Build the site using Azure Pipelines or Github Actions
* Execute the Unit Tests
* Deploy the API to the development slot of the Azure Web App
* Once successful, 'click to deploy' to production

Part I: Created the [`deploy-to-azure.yml`](/.github/workflows/deploy-to-azure.yml) file.

Created steps in the Action to

* Restore packages
* Build the solution
* Test the Solution
* Deploy the Web Application to Azure

Part II: Deployed the Web API and added a new Build Status badge

* [Video Part I](https://youtu.be/txuGf_M6NBE)
* [Video Part II](https://youtu.be/L-9EaIzb-1E)
* Source Code [96fad81](https://github.com/jguadagno/Contacts/commit/96fad81179bcebf16aa2f9c88c5a428b9c0f2381)

## Updated the Microsoft Identity Web package

There were some breaking changes with this update but otherwise it was painful.

* [Video](https://www.youtube.com/watch?v=-Sl8rz4fRQU)
* Source Code [07211dc](https://github.com/jguadagno/Contacts/commit/07211dc0d8b2dad6ad9ea0e989f5d4691966a8c9)

Implement the SQLServerCacheTokenProvider and update the package. 

## User Secrets

We migrated the code to use User Secrets.  No more connection strings in source.

* [Video](https://youtu.be/zLSyy2G6YDg)
* Source Code [fcdf12b](https://github.com/jguadagno/Contacts/commit/fcdf12bb74570a71417ecfabee826fc6ca2fe0e9)

## Uploading Images of Contacts

Let's take a look at what it would take to upload an image of our contacts to our server.  Once uploaded we can than save and serve them from blob storage.

ASP.NET Docs for [File Uploads](https://docs.microsoft.com/en-us/aspnet/core/mvc/models/file-uploads?view=aspnetcore-3.1#:~:text=%20Upload%20files%20in%20ASP.NET%20Core%20%201,are%20buffering%20and%20streaming.%20The%20entire...%20More%20)

* [Video](https://youtu.be/N5m9flz0BJI)
* Source code [29ac0a8](https://github.com/jguadagno/Contacts/commit/29ac0a8dae6d8cef7ad5e40486fb70bd296b8abc)

### Adding Azure Blog Storage

Getting starting adding the uploaded image to Azure Blob Storage

* Part 1 - Local [Video](https://youtu.be/t3aqdXEY_-k)
* Part 2 - Azure [Video](https://youtu.be/LGUug-C_qL4)
* Source Code [bf937d0](https://github.com/jguadagno/Contacts/commit/bf937d04e9a01b831577cf408f2875fb862da49a)

Read the blog post [Securing Azure Containers and Blobs with Managed Identities](https://www.josephguadagno.net/2020/08/22/securing-azure-containers-and-blobs-with-managed-identities) for more details.

#### Links

* Azure [Blob Storage](https://azure.microsoft.com/en-us/services/storage/blobs/)
* Storage Emulators
  * [Using Azurite](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-azurite)
  * [Using Azure Storage Explorer](https://docs.microsoft.com/en-us/azure/storage/common/storage-use-emulator)
* .NET SDK [Quickstart](https://docs.microsoft.com/en-us/azure/storage/blobs/storage-quickstart-blobs-dotnet)
* NuGet Package [JosephGuadagno.AzureHelpers.Storage](https://github.com/jguadagno/JosephGuadagno.AzureHelpers.Storage)

### Creating Thumbnails

* Part 1: [Video](https://youtu.be/HWHQRE0J3sc)
* Part 2: [Video](https://youtu.be/rD7Je7kBB9c)
* Part 3: [Video](https://www.youtube.com/watch?v=-SBcFNQXlTY)
* Source Code [8b4b65f](https://github.com/jguadagno/Contacts/commit/8b4b65f76992ee469e3c29c658efdde7b2626016)
* Final Source Code [6563ef6](https://github.com/jguadagno/Contacts/commit/6563ef6cd4019d892e0df6631127d96a044e3714)

### Adding a Content Delivery Network (CDN)

Let's put a CDN in front of our Azure Blob Storage container to server up images fast!

There was no code for this, just the video

* [Video](https://www.youtube.com/watch?v=Cf6Y69C5jUA)

## Refactoring Code

After building out the application we noticed that there are quite a few places that we have our imaging functions.  Let's go through and refactor the code to create an Image Manager that we can us in the WebUI and Azure Function.

* Creating the Image Manager Part 1 [Video](https://www.youtube.com/watch?v=DxzRUaMuN5U)
* Creating the Image Manager Part 2 [Video](https://www.youtube.com/watch?v=LrHcsQuAt6k)
* Creating the Image Manager Part Finale [Video](https://www.youtube.com/watch?v=OHyPbasWNEc)
* Source Code [b123c1c](https://github.com/jguadagno/Contacts/commit/b123c1cd6d386999aa2169fb9348fed7be91c3bc)

## Future UI Clients

Here are some other client technologies that we can use.
Remaining Options

### Native Application

#### React Native

#### Ionic client

### Blazor Client

### WinForms (Maybe)

## To the Cloud

Setup the environment

[Post](https://www.phillipsj.net/posts/infrastructure-in-csharp-using-the-azure-management-sdk/) on building infrastructure with code.

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

## Application Flexibility

### Data Persistence

Let's change out the database

#### Move to Json file

#### Move to AWS Dynamo

## Nice to Haves

A list of things that we might want to consider adding to the application

* [ADDING SECURITY TO OAS 3 / SWAGGER IN .NET CORE 3.1 USING SWASHBUCKLE](https://pradeeploganathan.com/api/add-security-requirements-oas3-swagger-netcore3-1-using-swashbuckle/)
* [Infrastructure in C# using the Azure Management SDK](https://www.phillipsj.net/posts/infrastructure-in-csharp-using-the-azure-management-sdk/)
