using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookWebApi.Mappings.DTOs.ContactDetail
{
    public class ContactDetailCreateDto
    {
        public string Description { get; set; }
        [MaxLength(100)]
        public string Address { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Telephone { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string Cell { get; set; }
        [MaxLength(64)]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
    }
}
