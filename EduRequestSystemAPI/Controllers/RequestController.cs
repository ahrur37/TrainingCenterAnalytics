using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduRequestSystemAPI.CustomAttributes;

namespace EduRequestSystemAPI.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        /// <summary>
        /// Создание новой заявки на обучение.
        /// </summary>
        /// <remarks>
        /// Доступно строго роли: Заявитель (1).
        /// Позволяет студенту сформировать и отправить новую заявку, указав тему, описание, направление и формат обучения.
        /// </remarks>
        /// <param name="createRequestModel">Модель с данными для создания заявки</param>
        [HttpPost]
        [RoleAuthorized(1)]
        [Route("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequest createRequestModel)
        {
            return await _requestService.CreateRequestAsync(createRequestModel);
        }

        /// <summary>
        /// Получение подробной информации о конкретной заявке по её ID.
        /// </summary>
        /// <remarks>
        /// Доступно ролям: Заявитель (1), Менеджер (2), Администратор (3), Руководитель (4).
        /// На уровне сервисной логики Заявитель сможет увидеть только свою собственную заявку, тогда как сотрудники (2, 3, 4) имеют доступ к любым заявкам.
        /// </remarks>
        /// <param name="requestId">Уникальный идентификатор (ID) запрашиваемой заявки</param>
        [HttpGet]
        [RoleAuthorized(1, 2, 3, 4)]
        [Route("GetRequestById/{requestId}")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            return await _requestService.GetRequestByIdAsync(requestId);
        }

        /// <summary>
        /// Получение списка заявок с возможностью фильтрации и поиска.
        /// </summary>
        /// <remarks>
        /// Доступно ролям: Заявитель (1), Менеджер (2), Администратор (3), Руководитель (4).
        /// Заявитель (1) в ответе получит только созданные им заявки. 
        /// Менеджеры, Администраторы и Руководители увидят глобальный список с возможностью фильтрации по менеджеру, автору, статусу и направлению.
        /// </remarks>
        /// <param name="statusId">Необязательный фильтр по ID статуса заявки</param>
        /// <param name="directionId">Необязательный фильтр по ID направления обучения</param>
        /// <param name="searchTerm">Поисковый запрос (поиск по теме или описанию заявки)</param>
        /// <param name="assigneeId">Необязательный фильтр по ID назначенного менеджера</param>
        /// <param name="authorId">Необязательный фильтр по ID автора заявки</param>
        [HttpGet]
        [RoleAuthorized(1, 2, 3, 4)]
        [Route("GetRequests")]
        public async Task<IActionResult> GetRequests(int? statusId, int? directionId, string? searchTerm, int? assigneeId, int? authorId)
        {
            return await _requestService.GetRequestsAsync(statusId, directionId, searchTerm, assigneeId, authorId);
        }

        /// <summary>
        /// Назначение ответственного менеджера на заявку.
        /// </summary>
        /// <remarks>
        /// Доступно ролям: Менеджер (2), Администратор (3).
        /// Позволяет закрепить за заявкой сотрудника учебного центра, который будет её обрабатывать.
        /// </remarks>
        /// <param name="requestId">ID заявки, на которую назначается менеджер</param>
        /// <param name="managerId">ID сотрудника, который становится ответственным</param>
        [HttpPost]
        [RoleAuthorized(2, 3)]
        [Route("AssignManager")]
        public async Task<IActionResult> AssignManager(int requestId, int managerId)
        {
            return await _requestService.AssignManagerAsync(requestId, managerId);
        }

        /// <summary>
        /// Изменение текущего статуса заявки.
        /// </summary>
        /// <remarks>
        /// Доступно ролям: Менеджер (2), Администратор (3).
        /// Переводит заявку на следующий этап жизненного цикла.
        /// </remarks>
        /// <param name="requestId">ID изменяемой заявки</param>
        /// <param name="newStatusId">ID нового статуса, который нужно установить</param>
        /// <param name="currentUserId">ID текущего авторизованного сотрудника, выполняющего операцию</param>
        [HttpPost]
        [RoleAuthorized(2, 3)]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int requestId, int newStatusId, int currentUserId)
        {
            return await _requestService.ChangeStatusAsync(requestId, newStatusId, currentUserId);
        }
    }
}
