using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Contacts.WebUi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;
using System.Text.Json;

namespace Contacts.WebUi.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        // HttpClient example based off of: https://docs.microsoft.com/en-us/dotnet/csharp/tutorials/console-webapiclient
        private static readonly HttpClient Client = new HttpClient();
        private readonly ITokenAcquisition _tokenAcquisition;

        private ISettings _settings;

        public ContactController(Settings settings, ITokenAcquisition tokenAcquisition)
        {
            _settings = settings;
            _tokenAcquisition = tokenAcquisition;
        }

        // GET
        [AuthorizeForScopes(Scopes = new []{"api://dc68a11f-d265-4e9c-8a24-abbbd3520f8a/" + Domain.Permissions.Contacts.List})]
        public async Task<IActionResult> Index()
        {
            // Check for Scope
            string[] scopes = {"api://dc68a11f-d265-4e9c-8a24-abbbd3520f8a/" + Domain.Permissions.Contacts.List};
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            
            // Build the HttpClient
            
            // Build the Url
            // https://localhost:5001/contacts
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var response = await Client.GetAsync("https://localhost:5001/contacts");

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Parse the Results
                var content = await response.Content.ReadAsStringAsync();
                
                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true,
                };
                
                List<Domain.Models.Contact> contacts =
                    JsonSerializer.Deserialize<List<Domain.Models.Contact>>(content, options);

                return View(contacts);
                
            }
                
            throw new HttpRequestException($"Invalid status code in the HttpResponseMessage: {response.StatusCode}.");

        }
    }
}