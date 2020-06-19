using System.Collections.Generic;
using System.Threading.Tasks;
using Contacts.Domain.Models;

namespace Contacts.Domain.Interfaces
{
    public interface IContactDataStore
    {
        Contact GetContact(int contactId);
        Task<Contact> GetContactAsync(int contactId);
        
        List<Contact> GetContacts();
        Task<List<Contact>> GetContactsAsync();
        
        List<Contact> GetContacts(string firstName, string lastName);
        Task<List<Contact>> GetContactsAsync(string firstName, string lastName);
        
        Contact SaveContact(Contact contact);
        Task<Contact> SaveContactAsync(Contact contact);
        
        bool DeleteContact(int contactId);
        Task<bool> DeleteContactAsync(int contactId);
        
        bool DeleteContact(Contact contact);
        Task<bool> DeleteContactAsync(Contact contact);

        List<Phone> GetContactPhones(int contactId);
        Task<List<Phone>> GetContactPhonesAsync(int contactId);
        
        Phone GetContactPhone(int contactId, int phoneId);
        Task<Phone> GetContactPhoneAsync(int contactId, int phoneId);

        List<Address> GetContactAddresses(int contactId);
        Task<List<Address>> GetContactAddressesAsync(int contactId);
        
        Address GetContactAddress(int contactId, int addressId);
        Task<Address> GetContactAddressAsync(int contactId, int addressId);
    }
}