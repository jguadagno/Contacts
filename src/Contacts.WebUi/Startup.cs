using Contacts.WebUi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.TokenCacheProviders.InMemory;
using Microsoft.Identity.Web.UI;

namespace Contacts.WebUi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddSignIn(Configuration);

            var initialScopes = new[]
            {
                "api://dc68a11f-d265-4e9c-8a24-abbbd3520f8a/" + Domain.Permissions.Contacts.Delete,
                "api://dc68a11f-d265-4e9c-8a24-abbbd3520f8a/" + Domain.Permissions.Contacts.List,
                "api://dc68a11f-d265-4e9c-8a24-abbbd3520f8a/" + Domain.Permissions.Contacts.Save,
                "api://dc68a11f-d265-4e9c-8a24-abbbd3520f8a/" + Domain.Permissions.Contacts.Search,
                "api://dc68a11f-d265-4e9c-8a24-abbbd3520f8a/" + Domain.Permissions.Contacts.View
            };
            // Token acquisition service based on MSAL.NET
            // and chosen token cache implementation
            services.AddWebAppCallsProtectedWebApi(Configuration, initialScopes)
                .AddInMemoryTokenCaches();
            
            services.AddControllersWithViews(options =>
            {
                var policy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
                options.Filters.Add(new AuthorizeFilter(policy));
            }).AddMicrosoftIdentityUI();
            
            services.AddRazorPages();

            var config = new Settings();
            Configuration.Bind("Settings", config);      //  <--- This
            services.AddSingleton(config);
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