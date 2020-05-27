using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using Contacts.Data.Sqlite.Models;
using Contacts.Domain.Interfaces;
using Contact = Contacts.Domain.Models.Contact;

namespace Contacts.Data.Sqlite
{
    [ExcludeFromCodeCoverage]
    public class SqliteDataStore: IContactDataStore
    {        
        private readonly ContactContext _contactContext;
        private readonly Mapper _mapper;

        public SqliteDataStore()
        {
            _contactContext = new ContactContext();
            
            var configuration = new MapperConfiguration(cfg => {
                cfg.AddProfile<ContactProfile>();
            });
            _mapper = new Mapper(configuration);
        }
        public Contact GetContact(int contactId)
        {
            var dbContact = _contactContext.Contacts.Find(contactId);
            var contact = _mapper.Map<Contact>(dbContact);
            return contact;
        }

        public List<Contact> GetContacts()
        {
            var contacts = _contactContext.Contacts.ToList();
            return _mapper.Map<List<Contact>>(contacts);
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

            var dbContact = _contactContext.Contacts
                .Where(contact => contact.LastName == lastName && contact.FirstName == firstName).ToList();
            return _mapper.Map<List<Contact>>(dbContact);
        }

        public bool SaveContact(Contact contact)
        {
            var dbContact = _mapper.Map<Sqlite.Models.Contact>(contact);
            _contactContext.Contacts.Add(dbContact);
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
            
            var dbContact = _mapper.Map<Sqlite.Models.Contact>(contact);
            
            _contactContext.Contacts.Remove(dbContact);
            return _contactContext.SaveChanges() != 0;
        }
    }
}