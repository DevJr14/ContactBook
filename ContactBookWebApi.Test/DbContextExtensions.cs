using ContactBookWebApi.Entities;
using ContactBookWebApi.Persistence;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContactBookWebApi.Test
{
    public static class DbContextExtensions
    {
        public static void Seed(this ContactDbContext contactBbContext)
        {
            contactBbContext.Contacts.Add(new Contact()
            {
                ContactId = 20,
                FirstName = "Thabiso",
                Surname = "Zuma",
                BirthDate = new DateTime(2000, 5, 23),
                UpdatedDate = DateTime.Now,
                ContactDetails = new List<ContactDetail>(3)
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 15,
                        Description = "Description one",
                        Address = "129 Barbra Street",
                        Cell = "0818238391",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    },
                    new ContactDetail()
                    {
                        ContactDetailId = 16,
                        Description = "Description two",
                        Address = "129 Barbra Street",
                        Cell = "0818238392",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    },
                    new ContactDetail()
                    {
                        ContactDetailId = 17,
                        Description = "Description three",
                        Address = "129 Barbra Street",
                        Cell = "0818238393",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    }
                }
            });

            contactBbContext.Contacts.Add(new Contact()
            {
                ContactId = 21,
                FirstName = "Tshepi",
                Surname = "Boipelo",
                BirthDate = new DateTime(1993, 2, 27),
                UpdatedDate = DateTime.Now,
                ContactDetails = new List<ContactDetail>(3)
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 18,
                        Description = "Description one",
                        Address = "129 Barbra Street",
                        Cell = "0818238391",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    },
                    new ContactDetail()
                    {
                        ContactDetailId = 19,
                        Description = "Description two",
                        Address = "129 Barbra Street",
                        Cell = "0818238392",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    },
                    new ContactDetail()
                    {
                        ContactDetailId = 20,
                        Description = "Description three",
                        Address = "129 Barbra Street",
                        Cell = "0818238393",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    }
                }
            });

            contactBbContext.Contacts.Add(new Contact()
            {
                ContactId = 22,
                FirstName = "Thabo",
                Surname = "Mbheki",
                BirthDate = new DateTime(1998, 3, 14),
                UpdatedDate = DateTime.Now,
                ContactDetails = new List<ContactDetail>(3)
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 21,
                        Description = "Description one",
                        Address = "129 Barbra Street",
                        Cell = "0818238391",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    },
                    new ContactDetail()
                    {
                        ContactDetailId = 22,
                        Description = "Description two",
                        Address = "129 Barbra Street",
                        Cell = "0818238392",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    },
                    new ContactDetail()
                    {
                        ContactDetailId = 23,
                        Description = "Description three",
                        Address = "129 Barbra Street",
                        Cell = "0818238393",
                        Email = "junior@gmail.com",
                        Telephone = "0129223491"
                    }
                }
            });

            contactBbContext.Contacts.Add(new Contact()
            {
                ContactId = 23,
                FirstName = "Nelson",
                Surname = "Mandela",
                BirthDate = new DateTime(1918, 7, 18),
                UpdatedDate = DateTime.Now,
                ContactDetails = null
            });

            contactBbContext.SaveChanges();
        }
    }
}
