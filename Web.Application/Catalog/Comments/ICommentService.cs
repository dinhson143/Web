using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Comments;

namespace Web.Application.Catalog.Comments
{
    public interface ICommentService
    {
        public Task<List<CommentViewModel>> GetAlladmin(string languageId);

        public Task<List<CommentViewModel>> GetAllweb(string languageId);

        public Task<int> Delete(int id);
    }
}