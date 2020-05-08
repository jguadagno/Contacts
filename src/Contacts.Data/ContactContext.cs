using System;
using Contacts.Domain;
using Microsoft.EntityFrameworkCore;

namespace Contacts.Data
{
    public class ContactContext: DbContext
    {
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<PhoneType> PhoneTypes { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=/Users/jguadagno/Projects/Databases/contacts.db");
    }
}