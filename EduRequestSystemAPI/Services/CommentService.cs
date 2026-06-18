using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Enums;
using EduRequestSystemAPI.Hubs;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace EduRequestSystemAPI.Services
{
    public class CommentService : ICommentService
    {
        private readonly ContextDb _context;
        private readonly IHubContext<CommentHub> _hub;

        public CommentService(ContextDb context, IHubContext<CommentHub> hub)
        {
            _context = context;
            _hub = hub;
        }

        public async Task<IActionResult> AddCommentAsync(CreateComment createCommentModel)
        {
            try
            {
                var requestExists = await _context.Requests.AnyAsync(r => r.Id == createCommentModel.RequestId);

                var newComment = new Comment
                {
                    Content = createCommentModel.Content,
                    RequestId = createCommentModel.RequestId,
                    AuthorId = createCommentModel.AuthorId, 
                    CreatedAt = DateTime.UtcNow
                };

                _context.Comments.Add(newComment);
                await _context.SaveChangesAsync();

                var auditLog = new AuditLog
                {
                    Action = AuditAction.AddComment,     
                    EntityName = AuditEntity.Comment,
                    EntityId = newComment.Id,           
                    UserId = createCommentModel.AuthorId,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Добавлен комментарий к заявке ID {createCommentModel.RequestId}"
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();

                var author = await _context.Users.FindAsync(newComment.AuthorId);

                await _hub.Clients
                    .Group($"request-{newComment.RequestId}")
                    .SendAsync("CommentAdded", new
                    {
                        id = newComment.Id,
                        content = newComment.Content,
                        authorId = newComment.AuthorId,
                        author = author == null ? null : new { author.Id, author.Name, author.Email },
                        requestId = newComment.RequestId,
                        createdAt = newComment.CreatedAt
                    });

                return new OkObjectResult(new { message = "Комментарий успешно добавлен", commentId = newComment.Id });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при добавлении комментария: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetCommentsByRequestIdAsync(int requestId)
        {
            try
            {
                var comments = await _context.Comments
                    .Where(c => c.RequestId == requestId)
                    .Include(c => c.Author)
                    .OrderBy(c => c.CreatedAt)
                    .ToListAsync();

                return new OkObjectResult(comments);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении комментариев: {ex.Message}");
            }
        }
    }
}
