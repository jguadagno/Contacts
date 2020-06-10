using System.Diagnostics.CodeAnalysis;

namespace Contacts.Data.Sqlite.Models
{
    [ExcludeFromCodeCoverage]

    public class Phone 
    {
        public int PhoneId { get; set; }
        public string PhoneNumber { get; set; }
        public string Extension { get; set; }

        public PhoneType PhoneType { get; set;}
        public virtual Contact Contact { get; set; }
    }
}
