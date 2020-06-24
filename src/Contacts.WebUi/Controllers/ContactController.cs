using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Contacts.WebUi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Web;

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
        public async Task<IActionResult> Index()
        {
            // Check for Scope
            string[] scopes = new string[]{Domain.Permissions.Contacts.List};
            string accessToken = await _tokenAcquisition.GetAccessTokenForUserAsync(scopes);
            
            // Build the HttpClient
            
            
            // Send the HttpRequest
            
            // Parse the Results
            // List<Contacts.Domain.Models.Contact>
            return View();
        }
    }
}