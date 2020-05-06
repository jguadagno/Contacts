using System;
using System.Collections.Generic;
using System.Data;
using Contacts.Domain;

namespace Contacts.Logic
{
    public class ContactManager
    {
        public Contact GetContact(int contactId)
        {
            throw new NotImplementedException();
        }

        public List<Contact> GetContacts()
        {
            throw new NotImplementedException();
        }

        public List<Contact> GetContacts(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "lastname is a required field");
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "firstName is a required field");
            }
            
            throw new NotImplementedException();
        }

        public int SaveContact(Contact contact)
        {
            // Data Validation
            // Null Checks
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "contact is a required field");
            }

            if (string.IsNullOrEmpty(contact.FirstName))
            {
                throw new ArgumentNullException(nameof(contact.FirstName), "firstName is a required field");
            }

            if (string.IsNullOrEmpty(contact.LastName))
            {
                throw new ArgumentNullException(nameof(contact.LastName), "lastName is a required field");
            }

            if (string.IsNullOrEmpty(contact.EmailAddress))
            {
                throw new ArgumentNullException(nameof(contact.EmailAddress), "EmailAddress is a required field");
            }
            
            // Business Rule Validation
            if (contact.Birthday > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(contact.Birthday), contact.Birthday,
                    "The birthday can not be in the future");
            }

            if (contact.Anniversary.HasValue && contact.Anniversary > DateTime.Now)
            {
                throw new ArgumentOutOfRangeException(nameof(contact.Anniversary), contact.Anniversary,
                    "The anniversary can not be in the future");
            }
            if (contact.Anniversary.HasValue && contact.Anniversary < contact.Birthday)
            {
                throw new ArgumentOutOfRangeException(nameof(contact.Anniversary), contact.Anniversary,
                    "The anniversary can not be earlier than the birthday.");
            }
            
            throw new NotImplementedException();
        }

        public bool DeleteContact(int contactId)
        {
            throw new NotImplementedException();
        }

        public bool DeleteContact(Contact contact)
        {
            // Data Validation
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "The contact parameter is required!");
            }
            
            return DeleteContact(contact.ContactId);
        }

    }
}