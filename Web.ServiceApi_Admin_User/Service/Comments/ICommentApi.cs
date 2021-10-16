using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.ViewModels.Catalog.Comments;
using Web.ViewModels.Catalog.Common;

namespace Web.ServiceApi_Admin_User.Service.Comments
{
    public interface ICommentApi
    {
        public Task<ResultApi<List<CommentViewModel>>> GetAllAdmin(string languageId, string BearerToken);

        public Task<ResultApi<List<CommentViewModel>>> GetAllWeb(string languageId);

        public Task<bool> Delete(int commentId, string BearerToken);

        public Task<ResultApi<string>> CreateComment(CommentCreate request, string BearerToken);
    }
}