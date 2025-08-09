using System;
using System.IO;
using System.Reflection;
using Contacts.Data;
using Contacts.Data.SqlServer;
using Contacts.Domain.Interfaces;
using Contacts.Logic;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Services);

var app = builder.Build();

if (builder.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

ConfigureMiddleware(app);
app.Run();


void ConfigureServices(IServiceCollection services)
{
    
    services.AddApplicationInsightsTelemetry();

    // Configure the logger
    var fullyQualifiedLogFile = Path.Combine(builder.Environment.ContentRootPath, "logs\\logs.txt");
    ConfigureLogging(builder.Configuration, builder.Services, fullyQualifiedLogFile, "Web");
    
    services.AddRazorPages();
    services.AddControllers();
    services.AddCors();

    services.AddEndpointsApiExplorer();
    // Register the Swagger generator, defining 1 or more Swagger documents
    services.AddSwaggerGen(c =>
    {
        c.SwaggerDoc("v1",
            new OpenApiInfo
            {
                Title = "Coding with JoeG Contact API", 
                Version = "v1",
                Description = "The API for the Contacts Application on Coding with JoeG",
                TermsOfService = new Uri("https://example.com/terms"),
                Contact = new OpenApiContact
                {
                    Name = "Joseph Guadagno",
                    Email = "jguadagno@hotmail.com",
                    Url = new Uri("https://www.josephguadagno.net"),
                }
            });
                
        // Set the comments path for the Swagger JSON and UI.
        var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
        var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
        c.IncludeXmlComments(xmlPath);
    });
            
    services.AddTransient<IContactDataStore, SqlServerDataStore>();
    //services.AddTransient<IContactDataStore, SqliteDataStore>();
    services.AddTransient<IContactRepository, ContactRepository>();
    services.AddTransient<IContactManager, ContactManager>();
}

void ConfigureMiddleware(IApplicationBuilder applicationBuilder)
{
            
    // TODO: Research and document for React Native client
    applicationBuilder.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("https://localhost:44311", "https://localhost:5001", "https://cwjg-contacts-web.azurewebsites.net"));
            
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    applicationBuilder.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    applicationBuilder.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coding with JoeG Contact Api V1");
    });

    applicationBuilder.UseHttpsRedirection();
    applicationBuilder.UseRouting();
        
    applicationBuilder.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers(); // Map attribute-routed API controllers
        endpoints.MapRazorPages();//map razor pages
    });
}


void ConfigureLogging(IConfigurationRoot configurationRoot, IServiceCollection services, string logPath, string applicationName)
{
    var logger = new LoggerConfiguration()
        .Enrich.FromLogContext()
        .Enrich.WithMachineName()
        .Enrich.WithThreadId()
        .Enrich.WithEnvironmentName()
        .Enrich.WithAssemblyName()
        .Enrich.WithAssemblyVersion(true)
        .Enrich.WithExceptionDetails()
        .Enrich.WithProperty("Application", applicationName)
        .Destructure.ToMaximumDepth(4)
        .Destructure.ToMaximumStringLength(100)
        .Destructure.ToMaximumCollectionCount(10)
        .WriteTo.Console()
        .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
        .WriteTo.MSSqlServer(
            connectionString: configurationRoot.GetConnectionString("ContactsDatabaseSqlServer"),
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = false, 
                AutoCreateSqlDatabase = false
            })
        .WriteTo.OpenTelemetry()
        .CreateLogger();
    services.AddLogging(loggingBuilder =>
    {
        loggingBuilder.AddApplicationInsights(configureTelemetryConfiguration: (config) =>
                config.ConnectionString =
                    configurationRoot["ApplicationInsights:ConnectionString"],
            configureApplicationInsightsLoggerOptions: (_) => { });loggingBuilder.AddApplicationInsights();
        loggingBuilder.AddSerilog(logger);
    });
}
