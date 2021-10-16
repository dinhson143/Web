using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Web.Application.Catalog.Comments;
using Web.ViewModels.Catalog.Comments;

namespace Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;

        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CommentCreate request)
        {
            var result = await _commentService.CreateComment(request);
            return Ok(result);
        }

        [HttpGet("danh-sach-admin")]
        public async Task<List<CommentViewModel>> GetAllAdmin(string languageID)
        {
            var result = await _commentService.GetAlladmin(languageID);
            return result;
        }

        [HttpGet("danh-sach-web")]
        [AllowAnonymous]
        public async Task<List<CommentViewModel>> GetAllWeb(string languageID)
        {
            var result = await _commentService.GetAllweb(languageID);
            return result;
        }

        [HttpDelete("Delete/{commentId}")]
        public async Task<IActionResult> Delete(int commentId)
        {
            var result = await _commentService.Delete(commentId);
            return Ok(result);
        }
    }
}