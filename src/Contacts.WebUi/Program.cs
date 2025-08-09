using System.IO;
using System.Linq;
using Contacts.Domain.Interfaces;
using Contacts.ImageManager;
using Contacts.WebUi.Models;
using Contacts.WebUi.Services;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.MSSqlServer;

var builder = WebApplication.CreateBuilder(args);

ConfigureServices(builder.Configuration, builder.Services, builder.Environment);

var app = builder.Build();
ConfigureMiddleware(app, builder.Environment);

app.Run();

void ConfigureServices(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment environment)
{
    var settings = new Settings();
    configuration.Bind("Settings", settings);
    
    services.AddSingleton(settings);

    // Configure the logger
    var fullyQualifiedLogFile = Path.Combine(builder.Environment.ContentRootPath, "logs\\logs.txt");
    ConfigureLogging(builder.Configuration, builder.Services, fullyQualifiedLogFile, "Web");
    
    services.AddHttpClient();
    services.TryAddScoped<IContactService, ContactService>();
    services.AddSingleton<IImageStore>(_ =>
    {
        var blobs = environment.IsDevelopment()
            ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactImageContainerName)
            : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactImageContainerName);
        return new ImageStore(blobs);
    });
    
    services.AddSingleton<IThumbnailImageStore>(_ =>
    {
        var blobs = environment.IsDevelopment()
            ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactThumbnailImageContainerName)
            : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactThumbnailImageContainerName);
        return new ThumbnailImageStore(blobs);
    });

    services.TryAddScoped<IImageManager, ImageManager>();
    
    // Register Thumbnail Create Queue
    services.TryAddScoped(_ => environment.IsDevelopment()
        ? new Queue(settings.ThumbnailQueueStorageAccount, settings.ThumbnailQueueName)
        : new Queue(settings.ThumbnailQueueStorageAccountName, null, settings.ThumbnailQueueName));
    
    services.AddApplicationInsightsTelemetry();
                
    var initialScopes = new[]
    {
        settings.ApiScopeUri + Contacts.Domain.Permissions.Contacts.Delete,
        settings.ApiScopeUri + Contacts.Domain.Permissions.Contacts.List,
        settings.ApiScopeUri + Contacts.Domain.Permissions.Contacts.Save,
        settings.ApiScopeUri + Contacts.Domain.Permissions.Contacts.Search,
        settings.ApiScopeUri + Contacts.Domain.Permissions.Contacts.View
    };
    // Token acquisition service based on MSAL.NET
    // and chosen token cache implementation
    services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
        .AddMicrosoftIdentityWebApp(configuration)
        .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
        .AddDistributedTokenCaches();
    
    services.AddDistributedSqlServerCache(options =>
    {
        options.ConnectionString = 
            configuration.GetConnectionString("ContactsDatabaseSqlServer");
        options.SchemaName = "dbo";
        options.TableName = "Cache";
    });
    
    services.AddControllersWithViews(options =>
    {
        var policy = new AuthorizationPolicyBuilder()
            .RequireAuthenticatedUser()
            .Build();
        options.Filters.Add(new AuthorizeFilter(policy));
    }).AddMicrosoftIdentityUI();
    
    services.AddContactService();

    services.AddControllersWithViews();
    services.AddRazorPages();
}

void ConfigureMiddleware(WebApplication application, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        application.UseDeveloperExceptionPage();
    }
    else
    {
        application.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        application.UseHsts();
    }

    application.UseHttpsRedirection();
    application.UseStaticFiles();

    app.UseAuthentication();
    app.UseAuthorization();

    application.MapDefaultControllerRoute();
    application.MapRazorPages();
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