using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using Contacts.WebUi.Models;
using Microsoft.AspNetCore.Mvc;
using Contacts.WebUi.Services;
using Microsoft.AspNetCore.Http;

namespace Contacts.WebUi.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly IImageManager _imageManager;
        private readonly Settings _settings;
        
        public ContactController(IContactService contactService, IImageManager imageManager, Settings settings)
        {
            _contactService = contactService;
            _imageManager = imageManager;
            _settings = settings;
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
            var savedContact = await _contactService.SaveContactAsync(contact);
            return RedirectToAction("Details", new {id = savedContact.ContactId});
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _contactService.DeleteContactAsync(id);

            if (result)
            {
                return RedirectToAction("Index");
            }

            return View();
        }
        
        public IActionResult Add()
        {
            return View(new Contacts.Domain.Models.Contact());
        }
        
        [HttpPost]
        public async Task<RedirectToActionResult> Add(Domain.Models.Contact contact)
        {
            var savedContact = await _contactService.SaveContactAsync(contact);
            return RedirectToAction("Details", new {id = savedContact.ContactId});
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int contactId, IFormFile uploadFile)
        {
            // Save the image with the Image Manager
            var imageUrl = await _imageManager.SaveImageAsync(uploadFile.OpenReadStream());
            
            // Update the ImageUrl
            var contact = await _contactService.GetContactAsync(contactId);
            contact.ImageUrl = $"{_settings.ContactImageUrl}{imageUrl}";
            var wasSaved = await _contactService.SaveContactAsync(contact);
            
            // Return the details
            return RedirectToAction("Details", new {id = contactId});
        }
    }
}