using AutoMapper;
using ContactBookWebApi.Entities;
using ContactBookWebApi.Mappings.DTOs.Contact;
using ContactBookWebApi.Mappings.DTOs.ContactDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookWebApi.Mappings
{
    public class ContactBookProfile : Profile
    {
        public ContactBookProfile()
        {
            CreateMap<Contact, ContactReadDto>();
            CreateMap<ContactReadDto, Contact>();
            CreateMap<ContactCreateDto, Contact>();

            CreateMap<ContactDetail, ContactDetailReadDto>();
            CreateMap<ContactDetailReadDto, ContactDetail>();
            CreateMap<ContactDetailCreateDto, ContactDetail>();
        }
    }
}
