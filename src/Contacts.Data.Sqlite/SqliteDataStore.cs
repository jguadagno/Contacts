using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using AutoMapper;
using Contacts.Data.Sqlite.Models;
using Contacts.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

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
        public Domain.Models.Contact GetContact(int contactId)
        {
            // TODO: Will will probably want to come back and remove the extra dependencies to a new set of endpoints
            var dbContact = _contactContext.Contacts
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .FirstOrDefault(c => c.ContactId == contactId);
            var contact = _mapper.Map<Domain.Models.Contact>(dbContact);
            return contact;
        }

        public List<Domain.Models.Contact> GetContacts()
        {
            var contacts = _contactContext.Contacts.ToList();
            return _mapper.Map<List<Domain.Models.Contact>>(contacts);
        }

        public List<Domain.Models.Contact> GetContacts(string firstName, string lastName)
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
            return _mapper.Map<List<Domain.Models.Contact>>(dbContact);
        }

        public Domain.Models.Contact SaveContact(Domain.Models.Contact contact)
        {
            var dbContact = _mapper.Map<Sqlite.Models.Contact>(contact);
            _contactContext.Contacts.Add(dbContact);

            var wasSaved = _contactContext.SaveChanges() != 0;
            if (wasSaved)
            {
                contact.ContactId = dbContact.ContactId;
                return contact;
            }
            return null;
        }

        public bool DeleteContact(int contactId)
        {
            var contact = _contactContext.Contacts.Find(contactId);
            _contactContext.Contacts.Remove(contact);
            return _contactContext.SaveChanges() != 0;
        }
        
        public bool DeleteContact(Domain.Models.Contact contact)
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

        public List<Domain.Models.Phone> GetContactPhones(int contactId)
        {
            var dbPhones = _contactContext.Phones
                .Where(p => p.Contact.ContactId == contactId).ToList();

            var phones = _mapper.Map<List<Domain.Models.Phone>>(dbPhones);
            return phones;
        }

        public Domain.Models.Phone GetContactPhone(int contactId, int phoneId)
        {
            var dbPhone = _contactContext.Phones
                .FirstOrDefault(p => p.Contact.ContactId == contactId && p.PhoneId == phoneId);
            var phone = _mapper.Map<Domain.Models.Phone>(dbPhone);
            return phone;
        }
        
        public List<Domain.Models.Address> GetContactAddresses(int contactId)
        {
            var dbAddresses = _contactContext.Addresses
                .Where(a => a.Contact.ContactId == contactId).ToList();

            var addresses = _mapper.Map<List<Domain.Models.Address>>(dbAddresses);
            return addresses;
        }

        public Domain.Models.Address GetContactAddress(int contactId, int addressId)
        {
            var dbAddress = _contactContext.Addresses
                .FirstOrDefault(a => a.Contact.ContactId == contactId && a.AddressId == addressId);
            var address = _mapper.Map<Domain.Models.Address>(dbAddress);
            return address;
        }
    }
}