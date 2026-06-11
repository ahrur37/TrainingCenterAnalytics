using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface ICommentService
    {
        // добавить комментарий к заявке
        Task<IActionResult> AddCommentAsync(CreateComment createCommentModel);

        // получить все комментарии к конкретной заявке
        Task<IActionResult> GetCommentsByRequestIdAsync(int requestId);
    }
}
