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
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.UI;

namespace Contacts.WebUi
{
    public class Startup
    {
        private readonly IWebHostEnvironment _environment;
        public Startup(IConfiguration configuration, IWebHostEnvironment environment)
        {
            Configuration = configuration;
            _environment = environment;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var settings = new Settings();
            Configuration.Bind("Settings", settings);
            
            //  For Project Tye
            var uri = Configuration.GetServiceUri("contacts-api", "https");
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
            
            services.AddSingleton<IImageStore>(provider =>
            {
                var blobs = _environment.IsDevelopment()
                    ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactImageContainerName)
                    : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactImageContainerName);
                return new ImageStore(blobs);
            });
            
            services.AddSingleton<IThumbnailImageStore>(provider =>
            {
                var blobs = _environment.IsDevelopment()
                    ? new Blobs(settings.ContactBlobStorageAccount, settings.ContactThumbnailImageContainerName)
                    : new Blobs(settings.ContactBlobStorageAccountName, null, settings.ContactThumbnailImageContainerName);
                return new ThumbnailImageStore(blobs);
            });

            services.AddSingleton<IImageManager, ImageManager.ImageManager>();
            
            // Register Thumbnail Create Queue
            services.AddSingleton(provider => _environment.IsDevelopment()
                ? new Queue(settings.ThumbnailQueueStorageAccount, settings.ThumbnailQueueName)
                : new Queue(settings.ThumbnailQueueStorageAccountName, null, settings.ThumbnailQueueName));
            
            services.AddApplicationInsightsTelemetry(settings.AppInsightsKey);
                        
            var initialScopes = new[]
            {
                settings.ApiScopeUri + Domain.Permissions.Contacts.Delete,
                settings.ApiScopeUri + Domain.Permissions.Contacts.List,
                settings.ApiScopeUri + Domain.Permissions.Contacts.Save,
                settings.ApiScopeUri + Domain.Permissions.Contacts.Search,
                settings.ApiScopeUri + Domain.Permissions.Contacts.View
            };
            // Token acquisition service based on MSAL.NET
            // and chosen token cache implementation
            services.AddAuthentication(OpenIdConnectDefaults.AuthenticationScheme)
                .AddMicrosoftIdentityWebApp(Configuration)
                .EnableTokenAcquisitionToCallDownstreamApi(initialScopes)
                .AddDistributedTokenCaches();
            
            services.AddDistributedSqlServerCache(options =>
            {
                options.ConnectionString = 
                    Configuration.GetConnectionString("ContactsDatabaseSqlServer");
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
            services.AddRazorPages();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
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

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapRazorPages();
            });
        }
    }
}