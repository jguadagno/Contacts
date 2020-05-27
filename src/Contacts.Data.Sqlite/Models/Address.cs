using System.Diagnostics.CodeAnalysis;

namespace Contacts.Domain.Models
{
    [ExcludeFromCodeCoverage]

    public class Address {
        
        public int AddressId { get; set; }
        public string StreetAddress { get; set; }
        public string SecondaryAddress { get; set; }
        public string Unit { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string PostalCode { get; set; }

        public AddressType AddressType { get; set; }
    }
}
