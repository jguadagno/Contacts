using System.Collections.Generic;
using Contacts.Domain.Models;

namespace Contacts.Domain.Interfaces
{
    public interface IContactRepository
    {
        Contact GetContact(int contactId);
        List<Contact> GetContacts();
        List<Contact> GetContacts(string firstName, string lastName);
        bool SaveContact(Contact contact);
        bool DeleteContact(int contactId);
        bool DeleteContact(Contact contact);
    }
}