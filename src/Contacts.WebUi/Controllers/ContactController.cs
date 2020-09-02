using System;
using System.Diagnostics;
using System.Text.Json;
using System.Threading.Tasks;
using Contacts.WebUi.Models;
using Microsoft.AspNetCore.Mvc;
using Contacts.WebUi.Services;
using JosephGuadagno.AzureHelpers.Storage;
using Microsoft.AspNetCore.Http;

namespace Contacts.WebUi.Controllers
{
    public class ContactController : Controller
    {
        private readonly IContactService _contactService;
        private readonly Blobs _blobs;
        private readonly Queue _queue;
        private readonly Settings _settings;
        
        public ContactController(IContactService contactService, Blobs blobs, Settings settings, Queue queue)
        {
            _contactService = contactService;
            _blobs = blobs;
            _settings = settings;
            _queue = queue;
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
            
            var fileExtension = new System.IO.FileInfo(uploadFile.FileName).Extension;
            var filename = $"{contactId}{fileExtension}";

            var blobContentInfo = await _blobs.UploadAsync(filename, uploadFile.OpenReadStream(), true);
            
            var imageUrl = $"{_settings.ContactImageUrl}{_settings.ContactImageContainerName}/{filename}";
            var contact = await _contactService.GetContactAsync(contactId);
            contact.ImageUrl = imageUrl;
            
            // Create the Thumbnail
            // ContactId,  ContainerName, filename
            var thumbnailCreateMessage = new Domain.Models.Messages.ImageToConvert
            {
                ContactId = contactId,
                ContainerName = _settings.ContactImageContainerName,
                ImageName = filename
            };
            
            var sendReceipt = await _queue.AddMessageWithBase64EncodingAsync(thumbnailCreateMessage);

            var wasSaved = await _contactService.SaveContactAsync(contact);
            
            return RedirectToAction("Details", new {id = contactId});
        }
    }
}