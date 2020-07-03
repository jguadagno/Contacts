using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Contacts.WebUi.Models;
using Microsoft.Extensions.Hosting;

namespace Contacts.WebUi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHostEnvironment _environment;

        public HomeController(ILogger<HomeController> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public IActionResult Index()
        {
            // HACK: This is a hack to avoid login issues since the Token is cached in memory
            // This means, we get an MsalUiRequiredException with the message of 
            // 'No account or login hint was passed to the AcquireTokenSilent call'
            // everytime we start the application.  Until we migrate to SqlServer or another distributed
            // caching means, we need to Delete all the cookies.
            
            // TODO: Stop deleting cookies once a distributed cache is used for the MSAL
            if (_environment.IsDevelopment())
            {
                foreach (var cookieKey in Request.Cookies.Keys)
                {
                    Response.Cookies.Delete(cookieKey);
                }
            }

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}