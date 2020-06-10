using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Contacts.Data.Sqlite.Models;

namespace Contacts.Data.Sqlite
{
    [ExcludeFromCodeCoverage]
    public class ContactContext: DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<PhoneType> PhoneTypes { get; set; }
        
        // TODO: Move this to appsettings.json
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=/Users/jguadagno/Projects/Databases/contacts.db");
    }
}