using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.ComponentModel.DataAnnotations;

namespace Contacts.Domain.Models
{
    public class Contact
    {
        public Contact() {
            Addresses = new List<Address>();
            Phones = new List<Phone>();
        }

        public int ContactId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [ExcludeFromCodeCoverage]
        public string MiddleName { get; set; }
        [Required]
        public string LastName { get; set; }
        [EmailAddress]
        public string EmailAddress { get; set; }
        [Required]
        public DateTime Birthday { get; set; }
        public DateTime? Anniversary { get; set; }
        [ExcludeFromCodeCoverage]
        [Url]
        public string ImageUrl { get; set; }
        [ExcludeFromCodeCoverage]
        public List<Address> Addresses { get; set;}
        [ExcludeFromCodeCoverage]
        public List<Phone> Phones {get; set;}

        public string FullName {
            get 
            {
                if (string.IsNullOrEmpty(FirstName) && string.IsNullOrEmpty(LastName) &&
                    string.IsNullOrEmpty(MiddleName))
                {
                    return "Could not determine the contact name";
                }

                if (string.IsNullOrEmpty(MiddleName))
                {
                    return $"{FirstName} {LastName}";
                }

                return $"{FirstName} {MiddleName} {LastName}";
            }
        }
    }
}
