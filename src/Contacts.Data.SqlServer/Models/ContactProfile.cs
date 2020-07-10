using AutoMapper;

namespace Contacts.Data.SqlServer.Models
{
    public class ContactProfile: Profile
    {
        public ContactProfile()
        {
            CreateMap<Domain.Models.Contact, Contact>();
            CreateMap<Domain.Models.Address, Address>();
            CreateMap<Domain.Models.AddressType, AddressType>();
            CreateMap<Domain.Models.Phone, Phone>();
            CreateMap<Domain.Models.PhoneType, PhoneType>();
            
            CreateMap<Contact, Domain.Models.Contact>();
            CreateMap<Address, Domain.Models.Address>();
            CreateMap<AddressType, Domain.Models.AddressType>();
            CreateMap<Phone, Domain.Models.Phone>();
            CreateMap<PhoneType, Domain.Models.PhoneType>();
        }
    }
}