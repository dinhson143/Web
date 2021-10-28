using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using MimeKit;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Common;
using Web.ViewModels.Catalog.Contacts;

namespace Web.Application.Catalog.Contacts
{
    public class ContactService : IContactService
    {
        private readonly AppDbContext _context;

        public ContactService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ResultApi<string>> CreateContact(ContactCreate request)
        {
            var contact = new Contact()
            {
                Email = request.Email,
                Message = request.Message,
                Name = request.Name,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Status = Status.Active
            };

            await _context.Contacts.AddAsync(contact);
            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm Contact thành công");
            }
            return new ResultErrorApi<string>("Thêm Contact thất bại");
        }

        public async Task<List<ContactViewModel>> GetAll()
        {
            var query = from c in _context.Contacts
                        where c.Status == Status.Active
                        select new { c };
            return await query.Select(x => new ContactViewModel()
            {
                Id = x.c.Id,
                Name = x.c.Name,
                Email = x.c.Email,
                Message = x.c.Message,
                PhoneNumber = x.c.PhoneNumber,
                Address = x.c.Address
            }).ToListAsync();
        }

        public async Task<int> UpdateContact(ContactViewModel request)
        {
            var Contact = await _context.Contacts.FirstOrDefaultAsync(x => x.Id == request.Id);

            if (Contact == null) throw new WebException($"Cannot find a contact with id: {request.Id}");

            Contact.Name = request.Name;
            Contact.Message = request.Message;
            Contact.PhoneNumber = request.PhoneNumber;
            Contact.Email = request.Email;
            return await _context.SaveChangesAsync();
        }

        public async Task<int> Delete(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);
            if (contact == null) throw new WebException($"Cannot find a contact with id: {id}");

            contact.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<ContactViewModel> GetContactById(int id)
        {
            var contact = await _context.Contacts.FindAsync(id);

            var data = new ContactViewModel()
            {
                Id = contact.Id,
                Name = contact.Name,
                Email = contact.Email,
                Message = contact.Message,
                PhoneNumber = contact.PhoneNumber
            };
            return data;
        }

        //public async Task<ResultApi<int>> SendMail(ContactSendMail request)
        //{
        //    var message = new MimeMessage();
        //    message.From.Add(new MailboxAddress("Khách hàng", request.Email));
        //    message.To.Add(new MailboxAddress("", Email));
        //    message.Subject = "Xác nhận reset mật khẩu !!!";
        //    message.Body = new TextPart("plain")
        //    {
        //        Text = "Mã xác nhận: " + r.ToString()
        //    };
        //    using (var client = new SmtpClient())
        //    {
        //        client.Connect("smtp.gmail.com", 587, false);
        //        client.Authenticate("dinhson14399@gmail.com", "tranthingocyen");
        //        client.Send(message);

        //        client.Disconnect(true);
        //    }

        //    return new ResultSuccessApi<int>()
        //    {
        //        Message = "Vui lòng kiểm tra mail !!!",
        //    };
        //}
    }
}