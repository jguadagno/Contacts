using System.Diagnostics.CodeAnalysis;

namespace Contacts.Data.SqlServer.Models
{
    [ExcludeFromCodeCoverage]

    public class AddressType
    {
        public int AddressTypeId { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

    }
}
