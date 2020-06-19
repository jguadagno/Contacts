using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Contacts.Domain.Interfaces;
using Contacts.Domain.Models;

namespace Contacts.Data
{
    [ExcludeFromCodeCoverage]
    public class ContactRepository : IContactRepository
    {
        private readonly IContactDataStore _contactDataStore;

        public ContactRepository(IContactDataStore contactDataStore)
        {
            _contactDataStore = contactDataStore;
        }

        public Contact GetContact(int contactId)
        {
            return _contactDataStore.GetContact(contactId);
        }
        
        public async Task<Contact> GetContactAsync(int contactId)
        {
            return await _contactDataStore.GetContactAsync(contactId);
        }

        public List<Contact> GetContacts()
        {
            return _contactDataStore.GetContacts();
        }
        
        public async Task<List<Contact>> GetContactsAsync()
        {
            return await _contactDataStore.GetContactsAsync();
        }

        public List<Contact> GetContacts(string firstName, string lastName)
        {
            return _contactDataStore.GetContacts(firstName, lastName);
        }
        
        public async Task<List<Contact>> GetContactsAsync(string firstName, string lastName)
        {
            return await _contactDataStore.GetContactsAsync(firstName, lastName);
        }

        public Contact SaveContact(Contact contact)
        {
            return _contactDataStore.SaveContact(contact);
        }
        
        public async Task<Contact> SaveContactAsync(Contact contact)
        {
            return await _contactDataStore.SaveContactAsync(contact);
        }

        public bool DeleteContact(int contactId)
        {
            return _contactDataStore.DeleteContact(contactId);
        }
        
        public async Task<bool> DeleteContactAsync(int contactId)
        {
            return await _contactDataStore.DeleteContactAsync(contactId);
        }

        public bool DeleteContact(Contact contact)
        {
            return _contactDataStore.DeleteContact(contact);
        }
        
        public async Task<bool> DeleteContactAsync(Contact contact)
        {
            return await _contactDataStore.DeleteContactAsync(contact);
        }

        public List<Phone> GetContactPhones(int contactId)
        {
            return _contactDataStore.GetContactPhones(contactId);
        }
        
        public async Task<List<Phone>> GetContactPhonesAsync(int contactId)
        {
            return await _contactDataStore.GetContactPhonesAsync(contactId);
        }

        public Phone GetContactPhone(int contactId, int phoneId)
        {
            return _contactDataStore.GetContactPhone(contactId, phoneId);
        }
        
        public async Task<Phone> GetContactPhoneAsync(int contactId, int phoneId)
        {
            return await _contactDataStore.GetContactPhoneAsync(contactId, phoneId);
        }

        public List<Address> GetContactAddresses(int contactId)
        {
            return _contactDataStore.GetContactAddresses(contactId);
        }
        
        public async Task<List<Address>> GetContactAddressesAsync(int contactId)
        {
            return await _contactDataStore.GetContactAddressesAsync(contactId);
        }

        public Address GetContactAddress(int contactId, int addressId)
        {
            return _contactDataStore.GetContactAddress(contactId, addressId);
        }
        
        public async Task<Address> GetContactAddressAsync(int contactId, int addressId)
        {
            return await _contactDataStore.GetContactAddressAsync(contactId, addressId);
        }
    }
}