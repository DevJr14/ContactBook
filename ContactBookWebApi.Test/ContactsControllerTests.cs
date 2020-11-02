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

            var controller = new ContactsController(serviceMocker.Object, mapperMock.Object);
            var actionResult = await controller.GetContactById(30);
        }
    }
}
