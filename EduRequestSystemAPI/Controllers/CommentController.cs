using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduRequestSystemAPI.CustomAttributes;

namespace EduRequestSystemAPI.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpPost]
        [RoleAuthorized(1, 2, 3)]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CreateComment createCommentModel)
        {
            return await _commentService.AddCommentAsync(createCommentModel);
        }

        [HttpGet]
        [RoleAuthorized(1, 2, 3, 4)]
        [Route("GetCommentsByRequestId/{requestId}")]
        public async Task<IActionResult> GetCommentsByRequestId(int requestId)
        {
            return await _commentService.GetCommentsByRequestIdAsync(requestId);
        }
    }
}
