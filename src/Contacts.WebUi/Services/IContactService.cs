using System.Collections.Generic;
using System.Threading.Tasks;

namespace Contacts.WebUi.Services
{
    public interface IContactService
    {
        Task<Domain.Models.Contact> GetContactAsync(int contactId);
        Task<List<Domain.Models.Contact>> GetContactsAsync();
        Task<List<Domain.Models.Contact>> GetContactsAsync(string firstName, string lastName);
        Task<Domain.Models.Contact> SaveContactAsync(Domain.Models.Contact contact);
        Task<bool> DeleteContactAsync(Domain.Models.Contact contact);
        Task<bool> DeleteContactAsync(int contactId);
        Task<Domain.Models.Phone> GetContactPhoneAsync(int contactId, int phoneId);
        Task<List<Domain.Models.Phone>> GetContactPhonesAsync(int contactId);
        Task<Domain.Models.Address> GetContactAddressAsync(int contactId, int phoneId);
        Task<List<Domain.Models.Address>> GetContactAddressesAsync(int contactId);
    }
}