using ContactBookWebApi.Mappings.DTOs.ContactDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookWebApi.Mappings.DTOs.Contact
{
    public class ContactCreateDto
    {
        public string FirstName { get; set; }
        public string Surname { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime UpdatedDate { get; set; }

        public ICollection<ContactDetailCreateDto> ContactDetails { get; set; }
    }
}
