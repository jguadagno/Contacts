using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore;
using Contacts.Data.SqlServer.Models;
using Microsoft.Extensions.Configuration;

namespace Contacts.Data.SqlServer
{
    [ExcludeFromCodeCoverage]
    public class ContactContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public ContactContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Phone> Phones { get; set; }
        public DbSet<AddressType> AddressTypes { get; set; }
        public DbSet<PhoneType> PhoneTypes { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlServer(_configuration.GetConnectionString("ContactsDatabaseSqlServer"));
    }
}