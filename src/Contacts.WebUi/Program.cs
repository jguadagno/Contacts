using System;
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
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;
using NLog;
using NLog.Web;
using WebApplication = Microsoft.AspNetCore.Builder.WebApplication;

var logger = LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    ConfigureServices(builder.Configuration, builder.Services, builder.Environment);

    var app = builder.Build();
    ConfigureMiddleware(app, builder.Environment);
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

void ConfigureServices(IConfiguration configuration, IServiceCollection services, IWebHostEnvironment environment)
{
    var settings = new Settings();
    configuration.Bind("Settings", settings);
            
    //  For Project Tye
    var uri = configuration.GetServiceUri("contacts-api", "https");
    // If the uri is null we assume Tye is not running.
    // If not null, assure Uri ends in /
    if (uri != null)
    {
        var url = uri.AbsoluteUri;
        if (!url.EndsWith("/"))
        {
            url += "/";
        }
        // https://localhost:5001/
        settings.ApiRootUri = url;
    }
    
    services.AddSingleton(settings);
            
    Console.WriteLine(settings.ApiRootUri);
    
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

    services.AddSingleton<IImageManager, ImageManager>();
    
    // Register Thumbnail Create Queue
    services.AddSingleton(_ => environment.IsDevelopment()
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

void ConfigureMiddleware(WebApplication app, IWebHostEnvironment env)
{
    if (env.IsDevelopment())
    {
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();

    app.UseRouting();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapDefaultControllerRoute();
    app.MapRazorPages();
}
