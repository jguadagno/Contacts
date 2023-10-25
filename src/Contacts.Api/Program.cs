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
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    ConfigureServices(builder.Configuration, builder.Services);

    var app = builder.Build();

    if (builder.Environment.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }

    ConfigureMiddleware(app, app.Services);
    app.Run();
}
catch (Exception exception)
{
    // NLog: catch setup errors
    logger.Error(exception, "Stopped program because of exception");
    throw;
}
finally
{
    // Ensure to flush and stop internal timers/threads before application-exit (Avoid segmentation fault on Linux)
    LogManager.Shutdown();
}

void ConfigureServices(ConfigurationManager configuration, IServiceCollection services)
{
    services.AddApplicationInsightsTelemetry();
            
    services.AddMicrosoftIdentityWebApiAuthentication(configuration);
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

void ConfigureMiddleware(IApplicationBuilder app, IServiceProvider services)
{
            
    // TODO: Research and document for React Native client
    app.UseCors(x => x
        .AllowAnyMethod()
        .AllowAnyHeader()
        .WithOrigins("https://localhost:44311", "https://localhost:5001", "https://cwjg-contacts-web.azurewebsites.net"));
            
    // Enable middleware to serve generated Swagger as a JSON endpoint.
    app.UseSwagger();

    // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
    // specifying the Swagger JSON endpoint.
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coding with JoeG Contact Api V1");
    });

    app.UseHttpsRedirection();
    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();
        
    app.UseEndpoints(endpoints =>
    {
        endpoints.MapControllers(); // Map attribute-routed API controllers
        endpoints.MapRazorPages();//map razor pages
    });
}
