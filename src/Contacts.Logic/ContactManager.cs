using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using Contacts.Domain.Models;

namespace Contacts.Logic
{
    public class ContactManager : IContactManager
    {
        private readonly IContactRepository _contactRepository;

        public ContactManager(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }
        public Contact GetContact(int contactId)
        {
            return _contactRepository.GetContact(contactId);
        }

        public async Task<Contact> GetContactAsync(int contactId)
        {
            return await _contactRepository.GetContactAsync(contactId);
        }

        public List<Contact> GetContacts()
        {
            return _contactRepository.GetContacts();
        }
        
        public async Task<List<Contact>> GetContactsAsync()
        {
            return await _contactRepository.GetContactsAsync();
        }

        public List<Contact> GetContacts(string firstName, string lastName)
        {
            ValidationForGetContacts(firstName, lastName);

            return _contactRepository.GetContacts(firstName, lastName);
        }
        
        public async Task<List<Contact>> GetContactsAsync(string firstName, string lastName)
        {
            ValidationForGetContacts(firstName, lastName);

            return await _contactRepository.GetContactsAsync(firstName, lastName);
        }

        private static void ValidationForGetContacts(string firstName, string lastName)
        {
            if (string.IsNullOrEmpty(firstName))
            {
                throw new ArgumentNullException(nameof(firstName), "FirstName is a required field");
            }

            if (string.IsNullOrEmpty(lastName))
            {
                throw new ArgumentNullException(nameof(lastName), "LastName is a required field");
            }
        }

        public Contact SaveContact(Contact contact)
        {
            // Data Validation
            ValidationForSaveContact(contact);

            return _contactRepository.SaveContact(contact);
        }
        
        public async Task<Contact> SaveContactAsync(Contact contact)
        {
            // Data Validation
            ValidationForSaveContact(contact);

            return await _contactRepository.SaveContactAsync(contact);
        }

        private static void ValidationForSaveContact(Contact contact)
        {
            // Null Checks
            if (contact == null)
            {
                throw new ArgumentNullException(nameof(contact), "Contact is a required field");
            }

            if (string.IsNullOrEmpty(contact.FirstName))
            {
                throw new ArgumentNullException(nameof(contact.FirstName), "FirstName is a required field");
            }

            if (string.IsNullOrEmpty(contact.LastName))
            {
                throw new ArgumentNullException(nameof(contact.LastName), "LastName is a required field");
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
        }

        public bool DeleteContact(int contactId)
        {
            return _contactRepository.DeleteContact(contactId);
        }
        
        public async Task<bool> DeleteContactAsync(int contactId)
        {
            return await _contactRepository.DeleteContactAsync(contactId);
        }

        public bool DeleteContact(Contact contact)
        {
            return contact != null && _contactRepository.DeleteContact(contact);
        }

        public async Task<bool> DeleteContactAsync(Contact contact)
        {
            return contact != null && await _contactRepository.DeleteContactAsync(contact);
        }

        public List<Phone> GetContactPhones(int contactId)
        {
            return _contactRepository.GetContactPhones(contactId);
        }
        
        public async Task<List<Phone>> GetContactPhonesAsync(int contactId)
        {
            return await _contactRepository.GetContactPhonesAsync(contactId);
        }
        
        public Phone GetContactPhone(int contactId, int phoneId)
        {
            return _contactRepository.GetContactPhone(contactId, phoneId);
        }
        
        public async Task<Phone> GetContactPhoneAsync(int contactId, int phoneId)
        {
            return await _contactRepository.GetContactPhoneAsync(contactId, phoneId);
        }
        
        public List<Address> GetContactAddresses(int contactId)
        {
            return _contactRepository.GetContactAddresses(contactId);
        }
        
        public async Task<List<Address>> GetContactAddressesAsync(int contactId)
        {
            return await _contactRepository.GetContactAddressesAsync(contactId);
        }
        
        public Address GetContactAddress(int contactId, int addressId)
        {
            return _contactRepository.GetContactAddress(contactId, addressId);
        }
        
        public async Task<Address> GetContactAddressAsync(int contactId, int addressId)
        {
            return await _contactRepository.GetContactAddressAsync(contactId, addressId);
        }
    }
}