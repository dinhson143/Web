using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Contacts;

namespace Web.ServiceApi_Admin_User.Service.Contacts
{
    public interface IContactApi
    {
        public Task<ResultApi<string>> CreateContact(ContactCreate request, string BearerToken);

        public Task<ResultApi<List<ContactViewModel>>> GetAll();

        public Task<bool> Update(ContactViewModel request, string BearerToken);

        public Task<bool> Delete(int contactId, string BearerToken);

        public Task<ContactViewModel> GetContactById(int id);
    }
}