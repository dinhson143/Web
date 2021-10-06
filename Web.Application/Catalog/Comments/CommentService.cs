using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Comments;

namespace Web.Application.Catalog.Comments
{
    public class CommentService : ICommentService
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;

        public CommentService(AppDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<int> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment == null) throw new WebException($"Cannot find a bình luận with id: {id}");

            comment.Status = Status.InActive;
            return await _context.SaveChangesAsync();
        }

        public async Task<List<CommentViewModel>> GetAlladmin(string languageId)
        {
            var list = from c in _context.Comments
                       select new { c };

            var listData = new List<CommentViewModel>();
            foreach (var item in list)
            {
                var product = await _context.Products.FindAsync(item.c.ProductId);
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == item.c.ProductId && x.LanguageId == languageId);
                var user = await _userManager.FindByIdAsync(item.c.UserId.ToString());
                var data = new CommentViewModel()
                {
                    Content = item.c.Content,
                    DateCreated = item.c.DateCreated,
                    Status = item.c.Status,
                    ParentId = item.c.ParentId,
                    TenSP = productTranslation.Name,
                    TenUser = user.UserName
                };

                listData.Add(data);
            }

            return (listData);
        }

        public async Task<List<CommentViewModel>> GetAllweb(string languageId)
        {
            var list = from c in _context.Comments
                       where c.Status == Status.Active
                       select new { c };

            var listData = new List<CommentViewModel>();
            foreach (var item in list)
            {
                var product = await _context.Products.FindAsync(item.c.ProductId);
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == item.c.ProductId && x.LanguageId == languageId);
                var user = await _userManager.FindByIdAsync(item.c.UserId.ToString());
                var data = new CommentViewModel()
                {
                    Content = item.c.Content,
                    DateCreated = item.c.DateCreated,
                    Status = item.c.Status,
                    ParentId = item.c.ParentId,
                    TenSP = productTranslation.Name,
                    TenUser = user.UserName
                };

                listData.Add(data);
            }

            return (listData);
        }
    }
}