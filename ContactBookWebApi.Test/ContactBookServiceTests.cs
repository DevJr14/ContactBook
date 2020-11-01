using ContactBookWebApi.Entities;
using ContactBookWebApi.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ContactBookWebApi.Test
{
    public class ContactBookServiceTests
    {
        [Fact]
        public async Task GetContactById_ContactExists_NotNull()
        {

            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(GetContactById_ContactExists_NotNull));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            Contact contact = await contactBookService.GetContactById(21);

            dbContext.Dispose();

            // Assert
            Assert.NotNull(contact);
        }

        [Fact]
        public async Task GetContactById_ContactDoesNotExists_Null()
        {

            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(GetContactById_ContactDoesNotExists_Null));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            Contact contact = await contactBookService.GetContactById(50);

            dbContext.Dispose();

            // Assert
            Assert.Null(contact);
        }

        [Fact]
        public async Task GetContactById_ContactWithContactDetailsExists_NotEmpty()
        {

            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(GetContactById_ContactWithContactDetailsExists_NotEmpty));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            Contact contact = await contactBookService.GetContactById(22);

            dbContext.Dispose();

            // Assert
            Assert.NotEmpty(contact.ContactDetails);
        }
        [Fact]
        public async Task GetContactById_ContactWithoutContactDetailExists_Empty()
        {

            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(GetContactById_ContactWithoutContactDetailExists_Empty));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            Contact contact = await contactBookService.GetContactById(23);

            dbContext.Dispose();

            // Assert
            Assert.Empty(contact.ContactDetails);
        }

        [Fact]
        public async Task SearchContactsByName_ContactsWithFirstNameExists_NotNull()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(SearchContactsByName_ContactsWithFirstNameExists_NotNull));
            ContactBookService contactBookService = new ContactBookService(dbContext);
            string firstName = "Thabo";

            // Act
            IEnumerable<Contact> contacts = await contactBookService.SearchContactsByName(firstName);

            dbContext.Dispose();

            // Assert
            Assert.NotNull(contacts);
            Assert.All(contacts, contact => contact.FirstName.Equals(firstName));
        }
        [Fact]
        public async Task SearchContactsByName_ContactsWithFirstNameDoesNotExists_Empty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(SearchContactsByName_ContactsWithFirstNameDoesNotExists_Empty));
            ContactBookService contactBookService = new ContactBookService(dbContext);
            string firstName = "Snipper";

            // Act
            IEnumerable<Contact> contacts = await contactBookService.SearchContactsByName(firstName);

            dbContext.Dispose();

            // Assert
            Assert.Empty(contacts);
        }
        [Fact]
        public async Task SearchContactsByName_ContactsWithSurnameExists_NotNull()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(SearchContactsByName_ContactsWithSurnameExists_NotNull));
            ContactBookService contactBookService = new ContactBookService(dbContext);
            string surname = "Mbheki";

            // Act
            IEnumerable<Contact> contacts = await contactBookService.SearchContactsByName(surname);

            dbContext.Dispose();

            // Assert
            Assert.NotNull(contacts);
            Assert.All(contacts, contact => contact.FirstName.Equals(surname));
        }
        [Fact]
        public async Task SearchContactsByName_ContactsWithSurnameDoesNotExists_Empty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(SearchContactsByName_ContactsWithSurnameDoesNotExists_Empty));
            ContactBookService contactBookService = new ContactBookService(dbContext);
            string surname = "Rubben";

            // Act
            IEnumerable<Contact> contacts = await contactBookService.SearchContactsByName(surname);

            dbContext.Dispose();

            // Assert
            Assert.Empty(contacts);
        }
        [Fact]
        public async Task SearchContactsByName_ContactsWithCellExists_NotEmpty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(SearchContactsByName_ContactsWithCellExists_NotEmpty));
            ContactBookService contactBookService = new ContactBookService(dbContext);
            string cell = "0818238392";

            // Act
            IEnumerable<Contact> contacts = await contactBookService.SearchContactsByName(cell);

            dbContext.Dispose();

            // Assert
            Assert.NotEmpty(contacts);
            Assert.All(contacts, contact => contact.ContactDetails.First().Cell.Equals(cell));
        }
        [Fact]
        public async Task SearchContactsByName_ContactsWithCellDoesNotExists_Empty()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(SearchContactsByName_ContactsWithCellDoesNotExists_Empty));
            ContactBookService contactBookService = new ContactBookService(dbContext);
            string cell = "0818238389";

            // Act
            IEnumerable<Contact> contacts = await contactBookService.SearchContactsByName(cell);

            dbContext.Dispose();

            // Assert
            Assert.Empty(contacts);
        }
        [Fact]
        public async Task ContactExists_ContactExists_True()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(ContactExists_ContactExists_True));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            int contactId = 22;
            bool response = await contactBookService.ContactExists(contactId);

            dbContext.Dispose();

            // Assert
            Assert.True(response);
        }
        [Fact]
        public async Task ContactExists_ContactDoesNotExists_False()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(ContactExists_ContactDoesNotExists_False));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            int contactId = 55;
            bool response = await contactBookService.ContactExists(contactId);

            dbContext.Dispose();

            // Assert
            Assert.False(response);
        }
        [Fact]
        public async Task FindAsync_ContactExists_NotNull()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(FindAsync_ContactExists_NotNull));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            int contactId = 22;

            Contact contact = await contactBookService.FindAsync(contactId);
            dbContext.Dispose();

            // Assert
            Assert.NotNull(contact);
            Assert.Equal(contactId, contact.ContactId);
        }
        [Fact]
        public async Task FindAsync_ContactDoesNotExists_Null()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(FindAsync_ContactDoesNotExists_Null));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            int contactId = 55;

            Contact contact = await contactBookService.FindAsync(contactId);
            dbContext.Dispose();

            // Assert
            Assert.Null(contact);
        }
        [Fact]
        public async Task GetContactDetail_ContactDetailExists_NotNull()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(GetContactDetail_ContactDetailExists_NotNull));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            int contactId = 22, contactDetailId = 21;

            ContactDetail contactDetail = await contactBookService.GetContactDetail(contactId, contactDetailId);
            dbContext.Dispose();

            // Assert
            Assert.NotNull(contactDetail);
        }
        [Fact]
        public async Task GetContactDetail_ContactDetailDoesNotExists_Null()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(GetContactDetail_ContactDetailDoesNotExists_Null));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            // Act
            int contactId = 22, contactDetailId = 30;

            ContactDetail contactDetail = await contactBookService.GetContactDetail(contactId, contactDetailId);
            dbContext.Dispose();

            // Assert
            Assert.Null(contactDetail);
        }
        [Fact]
        public async Task CreateContact_NewContactSuccessful_True()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(CreateContact_NewContactSuccessful_True));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            Contact contact = new Contact()
            {
                ContactId = 24,
                FirstName = "Jacob",
                Surname = "Ndlovu",
                BirthDate = new DateTime(1978, 11, 21),
                UpdatedDate = DateTime.Now,
                ContactDetails = new List<ContactDetail>()
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 24,
                        ContactId = 24,
                        Description = "Description for testing.",
                        Address = "Address for testing",
                        Cell = "0782829229",
                        Email = "test1@gmail.com",
                        Telephone = "0112348734"
                    }
                }
            };

            // Act
            await contactBookService.CreateContact(contact);
            await contactBookService.SaveAsync();

            bool isContactSaved = dbContext.Contacts.Any(c => c.ContactId.Equals(24) && c.FirstName.Equals("Jacob") && c.Surname.Equals("Ndlovu"));

            // Assert
            Assert.True(isContactSaved);
        }
        [Fact]
        public async Task CreateContact_NewContactDetailSuccessful_True()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(CreateContact_NewContactDetailSuccessful_True));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            Contact contact = new Contact()
            {
                ContactId = 25,
                FirstName = "Jacobs",
                Surname = "Ndlovus",
                BirthDate = new DateTime(1978, 11, 21),
                UpdatedDate = DateTime.Now,
                ContactDetails = new List<ContactDetail>()
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 25,
                        ContactId = 25,
                        Description = "Description for testing.",
                        Address = "Address for testing",
                        Cell = "0782829225",
                        Email = "test2@gmail.com",
                        Telephone = "0112348734"
                    }
                }
            };

            // Act
            await contactBookService.CreateContact(contact);
            await contactBookService.SaveAsync();

            bool isContactDetailSaved = dbContext.ContactDetails.Any(cd => cd.ContactId.Equals(25) && cd.ContactDetailId.Equals(25) && cd.Cell.Equals("0782829225") && cd.Email.Equals("test2@gmail.com"));

            // Assert
            Assert.True(isContactDetailSaved);
        }
        [Fact]
        public async Task DeleteContact_DeleteSuccessful_KeyNotFoundException()
        {
            // Arrange
            var dbContext = DbContextMocker.GetContactDbContext(nameof(DeleteContact_DeleteSuccessful_KeyNotFoundException));
            ContactBookService contactBookService = new ContactBookService(dbContext);

            Contact contact = new Contact()
            {
                ContactId = 26,
                FirstName = "Raymond",
                Surname = "Ndlovus",
                BirthDate = new DateTime(1978, 11, 21),
                UpdatedDate = DateTime.Now,
                ContactDetails = new List<ContactDetail>()
                {
                    new ContactDetail()
                    {
                        ContactDetailId = 26,
                        ContactId = 26,
                        Description = "Description for testing.",
                        Address = "Address for testing",
                        Cell = "0782829225",
                        Email = "test2@gmail.com",
                        Telephone = "0112348734"
                    }
                }
            };

            // Act 
            await contactBookService.CreateContact(contact);
            await contactBookService.SaveAsync();

            Contact delContact = await dbContext.Contacts.FindAsync(26);

            await contactBookService.DeleteContact(delContact);
            await contactBookService.SaveAsync();

            bool isDeleted = dbContext.Contacts.Any(c => c.ContactId.Equals(26));
            // Assert
            Assert.False(isDeleted);
        }
    }
}
