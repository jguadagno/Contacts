﻿using System;
using Contacts.Data;
using Contacts.Data.Sqlite;
using Contacts.Domain;
using Contacts.Domain.Models;
using Contacts.Logic;
using Microsoft.EntityFrameworkCore.Query.Internal;

namespace Contacts.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var contactManager = new ContactManager(new ContactRepository(new SqliteDataStore()));
            
            System.Console.WriteLine("--- Contact #1 ---");
            var searchedContact = contactManager.GetContact(1);
            if (searchedContact == null)
            {
                System.Console.WriteLine("Contact was not found");
            }
            else
            {
                System.Console.WriteLine(searchedContact.FullName);
            }
            
            System.Console.WriteLine("--- All Contacts ---");
            foreach (var contact in contactManager.GetContacts())
            {
                System.Console.WriteLine(contact.FullName);
            }

            System.Console.WriteLine("--- Search for Contact ---");
            var searchedForContacts = contactManager.GetContacts("Joseph", "Guadagno");
            if (searchedForContacts.Count == 0)
            {
                System.Console.WriteLine("No contacts were found!");
            }
            else
            {
                foreach (var searchResultsContact in searchedForContacts)
                {
                    System.Console.WriteLine(searchResultsContact.FullName);
                }
            }
            
            // NOTE: Skipping after the first successful add
            //System.Console.WriteLine("--- Adding Contact ---");
            //var wasSaved = contactManager.SaveContact(GetValidContact());
            //System.Console.Write(wasSaved ? "Contact was added": "Contact was NOT added");

            // NOTE: Skipping after the first successful delete
            //System.Console.WriteLine("--- Deleting Contact ---");
            //var wasDeleted = contactManager.DeleteContact(2);
            //System.Console.Write(wasDeleted ? "Contact was deleted": "Contact was NOT deleted");

        }

        private static Contact GetValidContact()
        {
            return new Contact
            {
                FirstName = "Deidre",
                MiddleName = "Ann",
                LastName = "Guadagno",
                EmailAddress = "donotemail@hotmail.com",
                Birthday = new DateTime(1900, 05, 03, 0,0,0)
            };
        }
    }
}