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
            var savedContact = contactManager.SaveContact(contact);

            if (savedContact != null)
            {
                return CreatedAtAction(nameof(GetContact), new {id = contact.ContactId},
                    contact);
            }
            return Problem("Failed to insert the contact");
        }

        [HttpDelete("{id}")]
        public ActionResult<bool> DeleteContact(int id)
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            var wasDeleted = contactManager.DeleteContact(id);
            if (wasDeleted)
            {
                return NoContent();
            }
            else
            {
                return NotFound();
            }
        }

        [HttpGet("search")]
        public List<Domain.Models.Contact> GetContacts([FromQuery]string firstname, [FromQuery]string lastname)
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            return contactManager.GetContacts(firstname, lastname);
        }
    }
}