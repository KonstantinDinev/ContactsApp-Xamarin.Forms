using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MyContactsMVVM.Models;

namespace MyContactsMVVM.ViewModels
{
    public interface IContactStore
    {
        Task<IEnumerable<Contact>> GetContactsAsync();
        Task<Contact> GetContact(int id);
        Task AddContact(Contact contact);
        Task UpdateContact(Contact contact);
        Task DeleteContact(Contact contact);
    }
}
