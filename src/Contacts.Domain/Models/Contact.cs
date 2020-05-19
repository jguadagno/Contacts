using System;
using System.Collections.Generic;

namespace Contacts.Domain.Models
{
    public class Contact
    {
        public Contact() {
            Addresses = new List<Address>();
            Phones = new List<Phone>();
        }

        public int ContactId { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime Birthday { get; set; }
        public DateTime? Anniversary { get; set; }
        public string ImageUrl { get; set; }
        public List<Address> Addresses { get; set;}
        public List<Phone> Phones {get; set;}

        public string FullName {
            get 
            {
                if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) &&
                    string.IsNullOrEmpty(MiddleName))
                {
                    return "Could not determine the contact name";
                }
                return string.IsNullOrEmpty(MiddleName)
                    ? $"{FirstName} {LastName}"
                    : $"{FirstName} {MiddleName} {LastName}";
            }
        }
    }
}
