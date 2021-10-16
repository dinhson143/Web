using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Contacts;

namespace Web.Application.Catalog.Contacts
{
    public interface IContactService
    {
        public Task<ResultApi<string>> CreateContact(ContactCreate request);

        public Task<List<ContactViewModel>> GetAll();

        public Task<int> UpdateContact(ContactViewModel request);

        public Task<int> Delete(int id);

        public Task<ContactViewModel> GetContactById(int id);
    }
}