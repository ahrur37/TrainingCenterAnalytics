using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly ContextDb _context;

        public CommentService(ContextDb context)
        {
            _context = context;
        }

        public Task<IActionResult> AddCommentAsync(CreateComment createCommentModel)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetCommentsByRequestIdAsync(int requestId)
        {
            throw new NotImplementedException();
        }
    }
}
