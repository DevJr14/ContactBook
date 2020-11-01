using ContactBookWebApi.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookWebApi.Repository
{
    public interface IContactBookService
    {
        Task<Contact> GetContactById(int id);
        Task<Contact> FindAsync(int id);
        Task CreateContact(Contact newContact);
        Task SaveAsync();
        void UpdateContact(Contact updateContact);
        Task<bool> ContactExists(int id);
        Task<ContactDetail> GetContactDetail(int contactId, int contactDetailId);
        Task<IEnumerable<Contact>> SearchContactsByName(string name);
        Task<IEnumerable<Contact>> GetFullContactAndContactDetails();
        Task DeleteContact(Contact delContact);
    }
}
