using System;
using System.Collections.Generic;
using System.Linq;
using Contacts.Data;
using Contacts.Domain;

namespace Contacts.Logic
{
    public class ContactManager
    {
        private ContactContext _contactContext;

        public ContactManager()
        {
            _contactContext = new ContactContext();
        }
        public Contact GetContact(int contactId)
        {
            return _contactContext.Contacts.Find(contactId);
        }

        public List<Contact> GetContacts()
        {
            return _contactContext.Contacts.ToList();
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

            return _contactContext.Contacts
                .Where(contact => contact.LastName == lastName && contact.FirstName == firstName).ToList();
        }

        public bool SaveContact(Contact contact)
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

            _contactContext.Contacts.Add(contact);
            return _contactContext.SaveChanges() != 0;
        }

        public bool DeleteContact(int contactId)
        {
            return DeleteContact(GetContact(contactId));
        }

        public bool DeleteContact(Contact contact)
        {
            // Data Validation
            if (contact == null)
            {
                return false;
            }
            
            _contactContext.Contacts.Remove(contact);
            return _contactContext.SaveChanges() != 0;
        }

    }
}