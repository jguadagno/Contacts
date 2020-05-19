using System.Collections.Generic;
using Contacts.Domain.Interfaces;
using Contacts.Domain.Models;

namespace Contacts.Data
{
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

    public List<Contact> GetContacts()
    {
        return _contactDataStore.GetContacts();
    }

    public List<Contact> GetContacts(string firstName, string lastName)
    {
        return _contactDataStore.GetContacts(firstName, lastName);
    }

    public bool SaveContact(Contact contact)
    {
        return _contactDataStore.SaveContact(contact);
    }

    public bool DeleteContact(int contactId)
    {
        return _contactDataStore.DeleteContact(contactId);
    }

    public bool DeleteContact(Contact contact)
    {
        return _contactDataStore.DeleteContact(contact);
    }
}
}