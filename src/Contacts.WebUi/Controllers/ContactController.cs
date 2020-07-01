using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Contacts.WebUi.Services;

namespace Contacts.WebUi.Controllers
{
    [Authorize]
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        public ContactController(IContactService contactService)
        {
            _contactService = contactService;
        }

        // GET All
        public async Task<IActionResult> Index()
        {
            var contacts = await _contactService.GetContactsAsync();

            return View(contacts);
        }

        // Get One (Details)
        public async Task<IActionResult> Details(int id)
        {
            var contact = await _contactService.GetContactAsync(id);

            return View(contact);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var contact = await _contactService.GetContactAsync(id);

            return View(contact);
        }

        [HttpPost]
        public async Task<RedirectToActionResult> Edit(Domain.Models.Contact contact)
        {
            var result = await _contactService.SaveContactAsync(contact);
            return RedirectToAction("Details", new {id = contact.ContactId});
        }
    }
}