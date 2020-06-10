using System.Collections.Generic;
using Contacts.Domain.Models;

namespace Contacts.Domain.Interfaces
{
    public interface IContactManager
    {
        Contact GetContact(int contactId);
        List<Contact> GetContacts();
        List<Contact> GetContacts(string firstName, string lastName);
        Contact SaveContact(Contact contact);
        bool DeleteContact(int contactId);
        bool DeleteContact(Contact contact);

        List<Phone> GetContactPhones(int contactId);
    }
}