using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.ServiceApi_Admin_User.Service.Comments;
using Web.Utilities.Contants;

namespace Web.AdminApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentApi _commentApi;

        public CommentController(ICommentApi commentApi)
        {
            _commentApi = commentApi;
        }

        public async Task<IActionResult> Index()
        {
            var languageId = HttpContext.Session.GetString(SystemContants.AppSettings.DefaultLanguageId);
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _commentApi.GetAllAdmin(languageId, token);
            return View(result.ResultObj);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int Id)
        {
            var token = HttpContext.Session.GetString(SystemContants.AppSettings.Token);
            var result = await _commentApi.Delete(Id, token);
            string message = "";
            if (result == true)
            {
                message = "Xóa thành công";
                TempData["Message"] = message;
            }
            else
            {
                message = "Xóa thất bại";
                TempData["Message"] = message;
            }
            return RedirectToAction("Index", "Comment");
        }
    }
}