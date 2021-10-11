using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Data.EF;
using Web.Data.Entities;
using Web.Data.Enums;
using Web.Utilities.Exceptions;
using Web.ViewModels.Catalog.Comments;
using Web.ViewModels.Catalog.Common;

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

        public async Task<ResultApi<string>> CreateComment(CommentCreate request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return null;
            }
            var kiemtraCM = await _context.Comments.FirstOrDefaultAsync(x => x.ProductId == request.ProductId && x.UserId == user.Id);
            if (kiemtraCM != null)
            {
                kiemtraCM.Content = request.Content;
                kiemtraCM.Star = request.Star;
                kiemtraCM.DateCreated = DateTime.Now;
            }
            else
            {
                var comment = new Comment()
                {
                    Content = request.Content,
                    DateCreated = DateTime.Now,
                    ProductId = request.ProductId,
                    Star = request.Star,
                    UserId = user.Id,
                    Status = Status.Active,
                    ParentId = request.ParentId
                };

                await _context.Comments.AddAsync(comment);
            }

            var result = await _context.SaveChangesAsync();
            if (result > 0)
            {
                return new ResultSuccessApi<string>("Thêm bình luận thành công");
            }
            return new ResultErrorApi<string>("Thêm bình luận thất bại");
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
                var data = new CommentViewModel()
                {
                    Content = item.c.Content,
                    DateCreated = item.c.DateCreated,
                    Status = item.c.Status,
                    ParentId = item.c.ParentId,
                    ProductId = item.c.ProductId,
                    UserId = item.c.UserId,
                    Star = item.c.Star
                };

                listData.Add(data);
            }
            foreach (var item in listData)
            {
                var user = await _userManager.FindByIdAsync(item.UserId.ToString());
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.LanguageId == languageId);
                item.TenSP = productTranslation.Name;
                item.TenUser = user.UserName;
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
                var data = new CommentViewModel()
                {
                    Content = item.c.Content,
                    DateCreated = item.c.DateCreated,
                    Status = item.c.Status,
                    ParentId = item.c.ParentId,
                    ProductId = item.c.ProductId,
                    UserId = item.c.UserId,
                    Star = item.c.Star
                };

                listData.Add(data);
            }
            foreach (var item in listData)
            {
                var user = await _userManager.FindByIdAsync(item.UserId.ToString());
                var productTranslation = await _context.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == item.ProductId && x.LanguageId == languageId);
                item.TenSP = productTranslation.Name;
                item.TenUser = user.UserName;
            }

            return (listData);
        }
    }
}