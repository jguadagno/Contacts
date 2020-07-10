using System.Diagnostics.CodeAnalysis;
using Contacts.Domain.Models;

namespace Contacts.Data.SqlServer.Models
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
        public virtual Contact Contact { get; set; }
    }
}
