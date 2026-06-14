using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface ICommentService
    {
        Task<IActionResult> AddCommentAsync(CreateComment createCommentModel); // добавить комментарий к заявке
        Task<IActionResult> GetCommentsByRequestIdAsync(int requestId); // получить все комментарии к конкретной заявке
    }
}
