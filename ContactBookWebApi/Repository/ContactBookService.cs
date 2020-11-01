using ContactBookWebApi.Entities;
using ContactBookWebApi.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookWebApi.Repository
{
    public class ContactBookService : IContactBookService
    {
        private readonly ContactDbContext _context;
        public ContactBookService(ContactDbContext context)
        {
            _context = context;
        }
        public async Task<bool> ContactExists(int id)
        {
            return await _context.Contacts.AnyAsync(c => c.ContactId == id);
        }

        public async Task CreateContact(Contact newContact)
        {
            if (newContact == null)
            {
                throw new ArgumentNullException();
            }
            //Check to prevent duplicate entries
            if (!IsDuplicateRecord(newContact.FirstName, newContact.Surname, newContact.BirthDate))
            {
                await _context.Contacts.AddAsync(newContact);
            }
            else
            {
                //Retreive existing record
                Contact existingContact = GetContactByName(newContact.FirstName, newContact.Surname);

                existingContact.ContactDetails.Add(newContact.ContactDetails.First());
                _context.Contacts.Update(existingContact);
            }
        }

        public async Task<Contact> GetContactById(int id)
        {
            var contact = await _context.Contacts.Include(c => c.ContactDetails)
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ContactId == id);
            return contact;
        }

        public async Task<IEnumerable<Contact>> GetFullContactAndContactDetails()
        {
            IQueryable<Contact> contacts = _context.Contacts.Join(_context.ContactDetails,
                c => c.ContactId,
                cd => cd.ContactId,
                (c, cd) => new { c, cd })
                .Select(con => new Contact
                {
                    ContactId = con.c.ContactId,
                    FirstName = con.c.FirstName,
                    Surname = con.c.Surname,
                    BirthDate = con.c.BirthDate,
                    UpdatedDate = con.c.UpdatedDate,
                    ContactDetails = new List<ContactDetail> { con.cd }
                });
            return await contacts.ToListAsync();
        }

        public async Task<ContactDetail> GetContactDetail(int contactId, int contactDetailId)
        {
            IQueryable<ContactDetail> contactDetails = _context.ContactDetails
                .Where(cd => cd.ContactDetailId == contactDetailId)
                .Include(c => c.Contact)
                .AsNoTracking();
            return await contactDetails.FirstOrDefaultAsync();
        }
        private Contact GetContactByName(string firstName, string surname)
        {
            return _context.Contacts.Include(c => c.ContactDetails).First(c => c.FirstName == firstName && c.Surname == surname);
        }

        private bool IsDuplicateRecord(string firstName, string surname, DateTime birthDate)
        {
            return _context.Contacts.Any(c => c.FirstName == firstName && c.Surname == surname && c.BirthDate == birthDate);
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Contact>> SearchContactsByName(string name)
        {
            IQueryable<Contact> contacts = _context.Contacts
                .Join(_context.ContactDetails,
                c => c.ContactId,
                cd => cd.ContactId,
                (c, cd) => new { c, cd })
                .Where(con => con.cd.Cell == name || con.c.FirstName == name || con.c.Surname == name)
                .Select(con => new Contact
                {
                    ContactId = con.c.ContactId,
                    FirstName = con.c.FirstName,
                    Surname = con.c.Surname,
                    BirthDate = con.c.BirthDate,
                    UpdatedDate = con.c.UpdatedDate,
                    ContactDetails = new List<ContactDetail> { con.cd }
                });
            return await contacts.ToListAsync();
        }

        public void UpdateContact(Contact updateContact)
        {
            var contactFromDb = _context.Contacts
                .Where(c => c.ContactId == updateContact.ContactId)
                .Include(c => c.ContactDetails)
                .AsNoTracking()
                .SingleOrDefault();

            _context.Entry(updateContact).State = EntityState.Modified;
            foreach (var contactDet in updateContact.ContactDetails)
            {
                bool cdExists = contactFromDb.ContactDetails.Any(cd => cd.ContactDetailId == contactDet.ContactDetailId);
                if (!cdExists)
                {
                    //When it does not exists, nothing to update.
                    return;
                }
                _context.Entry(contactDet).State = EntityState.Modified;
            }
        }

        public async Task<Contact> FindAsync(int id)
        {
            return await _context.Contacts.FindAsync(id);
        }

        public async Task DeleteContact(Contact delContact)
        {
            if (await ContactExists(delContact.ContactId))
            {
                _context.Contacts.Remove(delContact);
            }
        }

    }
}
