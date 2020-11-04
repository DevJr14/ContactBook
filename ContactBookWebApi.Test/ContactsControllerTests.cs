using AutoMapper;
using ContactBookWebApi.Controllers;
using ContactBookWebApi.Entities;
using ContactBookWebApi.Mappings.DTOs.Contact;
using ContactBookWebApi.Mappings.DTOs.ContactDetail;
using ContactBookWebApi.Repository;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ContactBookWebApi.Test
{
    public class ContactsControllerTests
    {
        [Fact]
        public async Task GetContactById_ContactExists_NotNull()
        {
            //What's in memory
            Contact contactFromService = new Contact()
            {
                ContactId = 30,
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetail>()
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 40,
                        ContactId = 30,
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            //What is expected
            ContactReadDto returnedContact = new ContactReadDto()
            {
                ContactId = 30,
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetailReadDto>()
                {
                    new ContactDetailReadDto()
                    {
                        ContactDetailId = 40,
                        ContactId = 30,
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.GetContactById(It.IsAny<int>()))
                .ReturnsAsync(contactFromService);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<ContactReadDto>(It.IsAny<Contact>()))
                .Returns(returnedContact);

            var controller = new ContactsController(serviceMocker.Object, mapperMock.Object);
            var actionResult = await controller.GetContactById(30);

            var result = actionResult.Result as OkObjectResult;
            Assert.NotNull(result.Value);
            Assert.Equal(returnedContact, result.Value);
        }

        [Fact]
        public async Task GetContactById_ContactDoesNotExists_NotFound()
        {
            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.GetContactById(It.IsAny<int>()))
                .ReturnsAsync((Contact)null);

            var mapperMock = new Mock<IMapper>();

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.GetContactById(30);
        }

        [Fact]
        public async Task GetAllContacts_MatchFound_NotNull()
        {
            //What's in memory
            IEnumerable<Contact> contactsFromService = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 30,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1978, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetail>()
                    {
                        new ContactDetail()
                        {
                            ContactDetailId = 40,
                            ContactId = 30,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selby@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
                new Contact()
                {
                    ContactId = 31,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1989, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetail>()
                    {
                        new ContactDetail()
                        {
                            ContactDetailId = 41,
                            ContactId = 31,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selbyo@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
            };

            //What's expected
            IEnumerable<ContactReadDto> returnedContacts = new List<ContactReadDto>() 
            {
                new ContactReadDto()
                {
                    ContactId = 30,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1978, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetailReadDto>()
                    {
                        new ContactDetailReadDto()
                        {
                            ContactDetailId = 40,
                            ContactId = 30,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selby@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
                new ContactReadDto()
                {
                    ContactId = 31,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1989, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetailReadDto>()
                    {
                        new ContactDetailReadDto()
                        {
                            ContactDetailId = 41,
                            ContactId = 31,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selby@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
            };

            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.SearchContactsByName("selby"))
                .ReturnsAsync(contactsFromService);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<ContactReadDto>>(It.IsAny<IEnumerable<Contact>>()))
                .Returns(returnedContacts);

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.GetAllContacts("selby");

            var result = actionResult.Result as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.Equal(returnedContacts, result.Value);
        }

        [Fact]
        public async Task GetAllContacts_MatchNotFound_Null()  
        {
            //What's in memory
            IEnumerable<Contact> contactsFromService = new List<Contact>()
            {
                new Contact()
                {
                    ContactId = 30,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1978, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetail>()
                    {
                        new ContactDetail()
                        {
                            ContactDetailId = 40,
                            ContactId = 30,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selby@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
                new Contact()
                {
                    ContactId = 31,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1989, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetail>()
                    {
                        new ContactDetail()
                        {
                            ContactDetailId = 41,
                            ContactId = 31,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selbyo@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
            };

            //What's expected
            IEnumerable<ContactReadDto> returnedContacts = new List<ContactReadDto>()
            {
                new ContactReadDto()
                {
                    ContactId = 30,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1978, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetailReadDto>()
                    {
                        new ContactDetailReadDto()
                        {
                            ContactDetailId = 40,
                            ContactId = 30,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selby@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
                new ContactReadDto()
                {
                    ContactId = 31,
                    FirstName = "Selby",
                    Surname = "Seroka",
                    BirthDate = new DateTime(1989, 7, 23),
                    UpdatedDate = new DateTime(2020, 11, 3),

                    ContactDetails = new List<ContactDetailReadDto>()
                    {
                        new ContactDetailReadDto()
                        {
                            ContactDetailId = 41,
                            ContactId = 31,
                            Address = "302 Lebea",
                            Cell = "0782347832",
                            Description = "Description",
                            Email = "selby@gmail.com",
                            Telephone = "0156728912"
                        }
                    }
                },
            };

            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.SearchContactsByName("selby"))
                .ReturnsAsync(contactsFromService);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<IEnumerable<ContactReadDto>>(It.IsAny<IEnumerable<Contact>>()))
                .Returns((IEnumerable<ContactReadDto>)null);

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.GetAllContacts("selby");

            var result = actionResult.Result as OkObjectResult;

            Assert.Null(result.Value);
        }
        [Fact]
        public async Task GetContactAndContactDetail_FoundMatchingContactId_NotNull()
        {
            //What's in memory
            Contact contactFromService = new Contact()
            {
                ContactId = 30,
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetail>()
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 40,
                        ContactId = 30,
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            //What is expected
            ContactReadDto returnedContact = new ContactReadDto()
            {
                ContactId = 30,
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetailReadDto>()
                {
                    new ContactDetailReadDto()
                    {
                        ContactDetailId = 40,
                        ContactId = 30,
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.ContactExists(30))
                .ReturnsAsync(true);

            serviceMocker
                .Setup(service => service.GetContactDetail(30, 40))
                .ReturnsAsync(contactFromService.ContactDetails.First());

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<ContactReadDto>(It.IsAny<ContactReadDto>()))
                .Returns(returnedContact);

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.GetContactAndContactDetails(30, 40);

            var result = actionResult.Result as OkObjectResult;

            Assert.NotNull(result.Value);
            Assert.Equal(returnedContact, result.Value);

        }

        [Fact]
        public async Task GetContactAndContactDetail_NotFoundMatchingContactId_NotFoundResult() 
        {           
            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.ContactExists(30))
                .ReturnsAsync(false);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<ContactReadDto>(It.IsAny<ContactReadDto>()));

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.GetContactAndContactDetails(30, 40);

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task GetContactAndContactDetail_NotFoundMatchingContactDetailId_NotFoundResult()  
        {
            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.ContactExists(30))
                .ReturnsAsync(true);

            serviceMocker
                .Setup(service => service.GetContactDetail(30, 40))
                .ReturnsAsync((ContactDetail)null);

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<ContactReadDto>(It.IsAny<ContactReadDto>()));

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.GetContactAndContactDetails(30, 40);

            Assert.IsAssignableFrom<NotFoundResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreateContact_SendNullValue_BadRequestResult()  
        {
            var serviceMocker = new Mock<IContactBookService>();
            var mapperMock = new Mock<IMapper>();

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.CreateContact(null);

            Assert.IsAssignableFrom<BadRequestResult>(actionResult.Result);
        }

        [Fact]
        public async Task CreateContact_Successful_NotNull()
        {
            ContactCreateDto newContact = new ContactCreateDto() 
            {
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetailCreateDto>()
                {
                    new ContactDetailCreateDto()
                    {
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            Contact contact = new Contact() 
            {
                ContactId = 30,
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetail>()
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 40,
                        ContactId = 30,
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            ContactReadDto returnedContact = new ContactReadDto() 
            {
                ContactId = 30,
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetailReadDto>()
                {
                    new ContactDetailReadDto()
                    {
                        ContactDetailId = 40,
                        ContactId = 30,
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            var mapperMock = new Mock<IMapper>();
            mapperMock
                .Setup(mapper => mapper.Map<Contact>(It.IsAny<ContactCreateDto>()))
                .Returns(contact);

            mapperMock
                .Setup(mapper => mapper.Map<ContactReadDto>(It.IsAny<Contact>()))
                .Returns(returnedContact);

            var serviceMocker = new Mock<IContactBookService>();
            serviceMocker
                .Setup(service => service.CreateContact(contact));

            serviceMocker
                .Setup(service => service.SaveAsync());

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.CreateContact(newContact);

            var result = actionResult.Result as CreatedAtActionResult;
            Assert.NotNull(result.Value);
            Assert.Equal(returnedContact, result.Value);
        }

        [Fact]
        public async Task UpdateContact_InConsistentContactId_BadRequestResult()
        {
            ContactReadDto contactForUpdate = new ContactReadDto()  
            {
                ContactId = 30,
                FirstName = "Selby",
                Surname = "Seroka",
                BirthDate = new DateTime(1978, 7, 23),
                UpdatedDate = new DateTime(2020, 11, 2),

                ContactDetails = new List<ContactDetailReadDto>()
                {
                    new ContactDetailReadDto()
                    {
                        ContactDetailId = 40,
                        ContactId = 30,
                        Address = "302 Lebea",
                        Cell = "0782347832",
                        Description = "Description",
                        Email = "selby@gmail.com",
                        Telephone = "0156728912"
                    }
                }
            };

            var mapperMock = new Mock<IMapper>();
            var serviceMocker = new Mock<IContactBookService>();

            var controller = new ContactsController(contactBookService: serviceMocker.Object, mapper: mapperMock.Object);
            var actionResult = await controller.UpdateContact(50, contactForUpdate);

            Assert.IsAssignableFrom<BadRequestResult>(actionResult);
        }
    }
}
