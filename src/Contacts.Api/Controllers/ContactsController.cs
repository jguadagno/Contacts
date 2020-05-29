using System;
using System.Collections.Generic;
using Contacts.Data;
using Contacts.Data.Sqlite;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Controllers
{
    // localhost:55555/Contacts/
    [ApiController]
    [Route("[controller]")]
    public class ContactsController: Controller
    {
        [HttpGet]
        public List<Domain.Models.Contact> GetContacts()
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            return contactManager.GetContacts();
        }
        
        [HttpGet("{id}")]
        public Domain.Models.Contact GetContact(int id)
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            return contactManager.GetContact(id);
        }

        [HttpPost]
        public ActionResult<Domain.Models.Contact> SaveContact(Domain.Models.Contact contact)
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            var wasSaved = contactManager.SaveContact(contact);

            if (wasSaved)
            {
                return CreatedAtAction(nameof(GetContact), new {id = contact.ContactId},
                    contact);
            }
            return Problem("Failed to insert the contact");
        }
        
    }
}