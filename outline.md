# Application Development Flow Outline

This is the development flow that we follow to build the contacts application

## Introduction to the IDE

Do we want to start with introducing Visual Studio? Visual Studio Code? JetBrains Rider?

Video: [Introduction to IDEs - Part 1](https://youtu.be/19nRZ6CBXDI): [Microsoft Visual Studio](https://visualstudio.microsoft.com/), [Microsoft Visual Studio Code](https://code.visualstudio.com/?wt.mc_id=DX_841432).

## Domain

### Model Development

### Business Layer

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
