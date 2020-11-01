﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookWebApi.Entities
{
    public class ContactDetail
    {
        public int ContactDetailId { get; set; }
        public int ContactId { get; set; }
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

        public Contact Contact { get; set; }
    }
}
