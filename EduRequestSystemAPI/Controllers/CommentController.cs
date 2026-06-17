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

        /// <summary>
        /// Добавление нового комментария к заявке.
        /// </summary>
        /// <remarks>
        /// Доступно ролям: Заявитель (1), Менеджер (2), Администратор (3).
        /// Позволяет вести диалог внутри заявки. Например, менеджер может запросить уточнения, а студент — ответить на них.
        /// </remarks>
        /// <param name="createCommentModel">Модель данных для создания комментария (ID заявки, ID автора, текст сообщения)</param>
        [HttpPost]
        [RoleAuthorized(1, 2, 3)]
        [Route("AddComment")]
        public async Task<IActionResult> AddComment([FromBody] CreateComment createCommentModel)
        {
            return await _commentService.AddCommentAsync(createCommentModel);
        }

        /// <summary>
        /// Получение всей цепочки комментариев по ID заявки.
        /// </summary>
        /// <remarks>
        /// Доступно ролям: Заявитель (1), Менеджер (2), Администратор (3), Руководитель (4).
        /// Необходимо вызывать этот метод сразу при открытии карточки заявки, чтобы подгрузить чат под детальной информацией.
        /// </remarks>
        /// <param name="requestId">Уникальный идентификатор заявки, для которой запрашивается переписка</param>
        [HttpGet]
        [RoleAuthorized(1, 2, 3, 4)]
        [Route("GetCommentsByRequestId/{requestId}")]
        public async Task<IActionResult> GetCommentsByRequestId(int requestId)
        {
            return await _commentService.GetCommentsByRequestIdAsync(requestId);
        }
    }
}
