using Azure.Storage.Blobs.Models;
using Contacts.WebUi.Models;
using Contacts.WebUi.Services;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.Distributed;
using Microsoft.Identity.Web.UI;

namespace Contacts.WebUi
{
    public class Startup
    {
        private IWebHostEnvironment _environment;
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
            services.AddSingleton(settings);

            // Register Contact Images Container
            services.AddSingleton(provider =>
            {
                Blobs blobs;
                if (_environment.IsDevelopment()) { 
                    blobs = new Blobs(settings.ContactBlobStorageAccount, settings.ContactImageContainerName);
                }
                else
                {
                    blobs = new Blobs("cwjgcontacts", null, settings.ContactImageContainerName);
                }

                var blob = blobs.BlobContainerClient.SetAccessPolicy(accessType: PublicAccessType.Blob);
                return blobs;
            });
            
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
            services.AddMicrosoftWebAppAuthentication(Configuration)
                .AddMicrosoftWebAppCallsWebApi(Configuration, initialScopes)
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