using System.Collections.Generic;
using Contacts.Domain.Interfaces;
using Contacts.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web.Resource;

namespace Contacts.Api.Controllers
{
    // https://localhost:5001/Contacts/
    
    /// <summary>
    /// The contacts endpoints provide the functionality to maintain our contacts.
    /// </summary>
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class ContactsController: Controller
    {

        private readonly IContactManager _contactManager;
        
        public ContactsController(IContactManager contactManager)
        {
            _contactManager = contactManager;
        }
        
        /// <summary>
        /// List all of the contacts currently available
        /// </summary>
        /// <returns>A List of <see cref="Contact"/></returns>
        /// <response code="200">Returns Ok</response>
        /// <response code="400">If requests is poorly formatted</response>            
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public List<Contact> GetContacts()
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.List);
            return _contactManager.GetContacts();
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
        public Contact GetContact(int id)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.View);
            return _contactManager.GetContact(id);
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
        public ActionResult<Contact> SaveContact(Contact contact)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.Save); 
            var savedContact = _contactManager.SaveContact(contact);

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
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.Delete);
            var wasDeleted = _contactManager.DeleteContact(id);
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
        public List<Contact> GetContacts([FromQuery]string firstname, [FromQuery]string lastname)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.Search);
            return _contactManager.GetContacts(firstname, lastname);
        }

        /// <summary>
        /// Gets phone numbers for the contact
        /// </summary>
        /// <param name="id">The primary identifier of the contact</param>
        /// <returns>A list of <see cref="Phone"/></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the request is poorly formatted</response>    
        [HttpGet("{id}/phones")]
        public List<Phone> GetContactPhones(int id)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.View);
            return _contactManager.GetContactPhones(id);
        }

        /// <summary>
        /// Gets a specific phone for the specified contact
        /// </summary>
        /// <param name="id">The primary identifier of the contact</param>
        /// <param name="phoneId">The primary identifier of the phone number</param>
        /// <returns>A <see cref="Contact"/></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the request is poorly formatted</response>    
        [HttpGet("{id}/phones/{phoneId}")]
        public Phone GetContactPhone(int id, int phoneId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.View);
            return _contactManager.GetContactPhone(id, phoneId);
        }
        
        /// <summary>
        /// Gets addresses for the contact
        /// </summary>
        /// <param name="id">The primary identifier of the contact</param>
        /// <returns>A list of <see cref="Address"/>es</returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the request is poorly formatted</response>    
        [HttpGet("{id}/addresses")]
        public List<Address> GetContactAddresses(int id)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.View);
            return _contactManager.GetContactAddresses(id);
        }

        /// <summary>
        /// Gets a specific phone for a specific contact\
        /// </summary>
        /// <param name="id">The primary identifier of the contact</param>
        /// <param name="addressId">The primary identifier of the address</param>
        /// <returns>A <see cref="Contact"/></returns>
        /// <response code="200">Ok</response>
        /// <response code="400">If the request is poorly formatted</response>    
        [HttpGet("{id}/addresses/{addressId}")]
        public Address GetContactAddress(int id, int addressId)
        {
            HttpContext.VerifyUserHasAnyAcceptedScope(Domain.Permissions.Contacts.View);
            return _contactManager.GetContactAddress(id, addressId);
        }
    }
}