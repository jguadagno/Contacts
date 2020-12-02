using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Contacts.Data.SqlServer.Models;
using Contacts.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Contact = Contacts.Domain.Models.Contact;

namespace Contacts.Data.SqlServer
{
    [ExcludeFromCodeCoverage]
    public class SqlServerDataStore: IContactDataStore
    {        
        private readonly ContactContext _contactContext;
        private readonly Mapper _mapper;

        public SqlServerDataStore(IConfiguration configuration)
        {
            _contactContext = new ContactContext(configuration);
            
            var mapperConfiguration = new MapperConfiguration(cfg => {
                cfg.AddProfile<ContactProfile>();
            });
            _mapper = new Mapper(mapperConfiguration);
        }
        
        public Contact GetContact(int contactId)
        {
            var dbContact = _contactContext.Contacts
                .FirstOrDefault(c => c.ContactId == contactId);
            var contact = _mapper.Map<Contact>(dbContact);
            return contact;
        }
        
        public async Task<Contact> GetContactAsync(int contactId)
        {
            var dbContact = await _contactContext.Contacts
                .FirstOrDefaultAsync(c => c.ContactId == contactId);
            var contact = _mapper.Map<Contact>(dbContact);
            return contact;
        }

        public List<Contact> GetContacts()
        {
            var contacts = _contactContext.Contacts.ToList();
            return _mapper.Map<List<Contact>>(contacts);
        }
        
        public async Task<List<Contact>> GetContactsAsync()
        {
            var contacts = await _contactContext.Contacts.ToListAsync();
            return _mapper.Map<List<Contact>>(contacts);
        }

        public List<Contact> GetContacts(string firstName, string lastName)
        {
            ValidationForGetContacts(firstName, lastName);

            var dbContact = _contactContext.Contacts
                .Where(contact => contact.LastName == lastName && contact.FirstName == firstName).ToList();
            return _mapper.Map<List<Contact>>(dbContact);
        }

        public async Task<List<Contact>> GetContactsAsync(string firstName, string lastName)
        {
            ValidationForGetContacts(firstName, lastName);

            var dbContact = await _contactContext.Contacts
                .Where(contact => contact.LastName == lastName && contact.FirstName == firstName).ToListAsync();
            return _mapper.Map<List<Contact>>(dbContact);
        }
        
        private static void ValidationForGetContacts(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "LastName is a required field");
            }

            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "FirstName is a required field");
            }
        }

        public Contact SaveContact(Contact contact)
        {
            var dbContact = _mapper.Map<SqlServer.Models.Contact>(contact);
            _contactContext.Entry(dbContact).State =
                dbContact.ContactId == 0 ? EntityState.Added : EntityState.Modified;

            var wasSaved = _contactContext.SaveChanges() != 0;
            if (wasSaved)
            {
                contact.ContactId = dbContact.ContactId;
                return contact;
            }
            return null;
        }
        
        public async Task<Contact> SaveContactAsync(Contact contact)
        {
            var dbContact = _mapper.Map<SqlServer.Models.Contact>(contact);

            _contactContext.Entry(dbContact).State =
                dbContact.ContactId == 0 ? EntityState.Added : EntityState.Modified;

            var wasSaved = await _contactContext.SaveChangesAsync() != 0;
            if (wasSaved)
            {
                contact.ContactId = dbContact.ContactId;
                return contact;
            }
            return null;
        }

        public bool DeleteContact(int contactId)
        {
            var contact =   _contactContext.Contacts
                .Include(c => c.Addresses)
                .Include(c => c.Phones)
                .FirstOrDefault(c => c.ContactId == contactId);

            if (contact == null)
            {
                return false;
            }
            
            _contactContext.Contacts.Remove(contact);
            foreach (var contactAddress in contact.Addresses)
            {
                _contactContext.Addresses.Remove(contactAddress);
            }

            foreach (var contactPhone in contact.Phones)
            {
                _contactContext.Phones.Remove(contactPhone);
            }

            return _contactContext.SaveChanges() != 0;
        }
        
        public async Task<bool> DeleteContactAsync(int contactId)
        {
            var contact =  await _contactContext.Contacts
                    .Include(c => c.Addresses)
                    .Include(c => c.Phones)
                    .FirstOrDefaultAsync(c => c.ContactId == contactId);

            if (contact == null)
            {
                return false;
            }
            
            _contactContext.Contacts.Remove(contact);
            foreach (var contactAddress in contact.Addresses)
            {
                _contactContext.Addresses.Remove(contactAddress);
            }

            foreach (var contactPhone in contact.Phones)
            {
                _contactContext.Phones.Remove(contactPhone);
            }

            return await _contactContext.SaveChangesAsync() != 0;
        }
        
        public bool DeleteContact(Contact contact)
        {
            return ValidateDeleteContact(contact) && DeleteContact(contact.ContactId);
        }
        
        public async Task<bool> DeleteContactAsync(Contact contact)
        {
            return ValidateDeleteContact(contact) && await DeleteContactAsync(contact.ContactId);
        }

        private static bool ValidateDeleteContact(Contact contact)
        {
            return contact != null;
        }

        public List<Domain.Models.Phone> GetContactPhones(int contactId)
        {
            var dbPhones = _contactContext.Phones
                .Where(p => p.Contact.ContactId == contactId).ToList();

            var phones = _mapper.Map<List<Domain.Models.Phone>>(dbPhones);
            return phones;
        }

        public async Task<List<Domain.Models.Phone>> GetContactPhonesAsync(int contactId)
        {
            var dbPhones = await _contactContext.Phones
                .Where(p => p.Contact.ContactId == contactId).ToListAsync();

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
        
        public async Task<Domain.Models.Phone> GetContactPhoneAsync(int contactId, int phoneId)
        {
            var dbPhone = await _contactContext.Phones
                .FirstOrDefaultAsync(p => p.Contact.ContactId == contactId && p.PhoneId == phoneId);
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
        
        public async Task<List<Domain.Models.Address>> GetContactAddressesAsync(int contactId)
        {
            var dbAddresses = await _contactContext.Addresses
                .Where(a => a.Contact.ContactId == contactId).ToListAsync();

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
        
        public async Task<Domain.Models.Address> GetContactAddressAsync(int contactId, int addressId)
        {
            var dbAddress = await _contactContext.Addresses
                .FirstOrDefaultAsync(a => a.Contact.ContactId == contactId && a.AddressId == addressId);
            var address = _mapper.Map<Domain.Models.Address>(dbAddress);
            return address;
        }
    }
}