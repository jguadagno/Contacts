using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Contacts.Domain.Interfaces;
using Contacts.Domain.Models;

namespace Contacts.Data.Sqlite
{
    [ExcludeFromCodeCoverage]
    public class SqliteDataStore: IContactDataStore
    {        
        private readonly ContactContext _contactContext;

        public SqliteDataStore()
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
                throw new ArgumentNullException(nameof(lastName), "LastName is a required field");
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "FirstName is a required field");
            }

            return _contactContext.Contacts
                .Where(contact => contact.LastName == lastName && contact.FirstName == firstName).ToList();
        }

        public bool SaveContact(Contact contact)
        {
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