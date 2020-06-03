using System.Collections.Generic;
using Contacts.Data;
using Contacts.Data.Sqlite;
using Contacts.Domain.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Contacts.Api.Controllers
{
    // https://localhost:5001/Contacts/
    
    /// <summary>
    /// The contacts endpoints provide the functionality to maintain our contacts.
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class ContactsController: Controller
    {
        /// <summary>
        /// List all of the contacts currently available
        /// </summary>
        /// <returns>A List of <see cref="Contact"/></returns>
        /// <response code="200">Returns Ok</response>
        /// <response code="400">If requests is poorly formatted</response>            
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public List<Domain.Models.Contact> GetContacts()
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            return contactManager.GetContacts();
        }
        
        /// <summary>
        /// Gets a specific contact from the contact manager
        /// </summary>
        /// <param name="id">The primary identifier of the contact</param>
        /// <returns>A <see cref="Contact"/></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the request is poorly formatted</response>            
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public Domain.Models.Contact GetContact(int id)
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            return contactManager.GetContact(id);
        }

        /// <summary>
        /// Adds a contact to the contact manager
        /// </summary>
        /// <param name="contact">A contact</param>
        /// <returns>The contact with the Url to view its details</returns>
        /// <response code="201">Returns the newly created item</response>
        /// <response code="400">If the item is null or there are data violations</response>            
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Deletes the specified contact
        /// </summary>
        /// <param name="id">The primary identifier for the contact</param>
        /// <returns></returns>
        /// <response code="200">If the item was deleted</response>
        /// <response code="400">If the request is poorly formatted</response>            
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
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

        /// <summary>
        /// Searches for a contact
        /// </summary>
        /// <param name="firstname">The first name of the contact to search for</param>
        /// <param name="lastname">The last name of the contact to search for</param>
        /// <returns>A list of 0 or more contacts that meet the criteria</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the request is poorly formatted</response>            
        [HttpGet("search")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public List<Domain.Models.Contact> GetContacts([FromQuery]string firstname, [FromQuery]string lastname)
        {
            var contactManager = new Logic.ContactManager(new ContactRepository(new SqliteDataStore()));
            return contactManager.GetContacts(firstname, lastname);
        }
    }
}