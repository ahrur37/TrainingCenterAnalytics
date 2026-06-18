using EduRequestSystemAPI.CustomAttributes;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Controllers
{
    [RoleAuthorized(1, 2, 3, 4)]
    public class DictionariesController : Controller
    {
        private readonly IDictionariesService _dictionariesService;

        public DictionariesController(IDictionariesService dictionariesService)
        {
            _dictionariesService = dictionariesService;
        }

        /// <summary>
        /// Получение списка всех направлений обучения.
        /// </summary>
        /// <remarks>
        /// Доступно всем авторизованным ролям (1, 2, 3, 4).
        /// Используется для заполнения выпадающих списков на фронтенде при создании заявки или фильтрации.
        /// </remarks>
        [HttpGet]
        [Route("GetDirections")]
        public async Task<IActionResult> GetDirections()
        {
            return await _dictionariesService.GetDirectionsAsync();
        }

        /// <summary>
        /// Создание нового направления обучения.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPost]
        [RoleAuthorized(3, 4)]
        [Route("CreateDirection")]
        public async Task<IActionResult> CreateDirection([FromBody] DictionaryModel model)
        {
            return await _dictionariesService.CreateDirectionAsync(model);
        }

        /// <summary>
        /// Редактирование существующего направления обучения.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPut]
        [RoleAuthorized(3, 4)]
        [Route("UpdateDirection/{id}")]
        public async Task<IActionResult> UpdateDirection(int id, [FromBody] DictionaryModel model)
        {
            return await _dictionariesService.UpdateDirectionAsync(id, model);
        }

        /// <summary>
        /// Удаление направления обучения.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpDelete]
        [RoleAuthorized(3, 4)]
        [Route("DeleteDirection/{id}")]
        public async Task<IActionResult> DeleteDirection(int id)
        {
            return await _dictionariesService.DeleteDirectionAsync(id);
        }

        /// <summary>
        /// Получение списка всех форматов обучения.
        /// </summary>
        /// <remarks>
        /// Доступно всем авторизованным ролям (1, 2, 3, 4).
        /// Возвращает доступные форматы (например: Очный, Дистанционный, Вебинар).
        /// </remarks>
        [HttpGet]
        [Route("GetTrainingFormats")]
        public async Task<IActionResult> GetTrainingFormats()
        {
            return await _dictionariesService.GetTrainingFormatsAsync();
        }

        /// <summary>
        /// Получение списка всех возможных статусов заявки.
        /// </summary>
        /// <remarks>
        /// Доступно всем авторизованным ролям (1, 2, 3, 4).
        /// Возвращает справочник статусов (например: Новая, В работе, Отклонена, Согласована).
        /// </remarks>
        [HttpGet]
        [Route("GetStatuses")]
        public async Task<IActionResult> GetStatuses()
        {
            return await _dictionariesService.GetStatusesAsync();
        }

        /// <summary>
        /// Создание нового формата обучения.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPost]
        [RoleAuthorized(3, 4)]
        [Route("CreateTrainingFormat")]
        public async Task<IActionResult> CreateTrainingFormat([FromBody] DictionaryModel model)
        {
            return await _dictionariesService.CreateTrainingFormatAsync(model);
        }

        /// <summary>
        /// Редактирование существующего формата обучения.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPut]
        [RoleAuthorized(3, 4)]
        [Route("UpdateTrainingFormat/{id}")]
        public async Task<IActionResult> UpdateTrainingFormat(int id, [FromBody] DictionaryModel model)
        {
            return await _dictionariesService.UpdateTrainingFormatAsync(id, model);
        }

        /// <summary>
        /// Удаление формата обучения.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpDelete]
        [RoleAuthorized(3, 4)]
        [Route("DeleteTrainingFormat/{id}")]
        public async Task<IActionResult> DeleteTrainingFormat(int id)
        {
            return await _dictionariesService.DeleteTrainingFormatAsync(id);
        }

        /// <summary>
        /// Создание нового статуса для заявок.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPost]
        [RoleAuthorized(3, 4)]
        [Route("CreateStatus")]
        public async Task<IActionResult> CreateStatus([FromBody] DictionaryModel model)
        {
            return await _dictionariesService.CreateStatusAsync(model);
        }

        /// <summary>
        /// Редактирование существующего статуса.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPut]
        [RoleAuthorized(3, 4)]
        [Route("UpdateStatus/{id}")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] DictionaryModel model)
        {
            return await _dictionariesService.UpdateStatusAsync(id, model);
        }

        /// <summary>
        /// Удаление статуса.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpDelete]
        [RoleAuthorized(3, 4)]
        [Route("DeleteStatus/{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            return await _dictionariesService.DeleteStatusAsync(id);
        }

        /// <summary>
        /// Получение списка всех пользовательских ролей в системе.
        /// </summary>
        /// <remarks>
        /// Доступно всем авторизованным ролям (1, 2, 3, 4).
        /// Возвращает роли: 1 - Заявитель, 2 - Менеджер, 3 - Админ, 4 - Руководитель.
        /// </remarks>
        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return await _dictionariesService.GetRolesAsync();
        }

        /// <summary>
        /// Создание новой роли.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPost]
        [RoleAuthorized(3, 4)]
        [Route("CreateRole")]
        public async Task<IActionResult> CreateRole([FromBody] DictionaryModel model)
        {
            return await _dictionariesService.CreateRoleAsync(model);
        }

        /// <summary>
        /// Редактирование названия роли.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpPut]
        [RoleAuthorized(3, 4)]
        [Route("UpdateRole/{id}")]
        public async Task<IActionResult> UpdateRole(int id, [FromBody] DictionaryModel model)
        {
            return await _dictionariesService.UpdateRoleAsync(id, model);
        }

        /// <summary>
        /// Удаление роли.
        /// </summary>
        /// <remarks>Доступно только роли: Администратор (3).</remarks>
        [HttpDelete]
        [RoleAuthorized(3, 4)]
        [Route("DeleteRole/{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            return await _dictionariesService.DeleteRoleAsync(id);
        }

        /// <summary>
        /// Получение списка абсолютно всех зарегистрированных пользователей системы.
        /// </summary>
        /// <remarks>
        /// Доступ ограничен: Менеджер (2), Администратор (3).
        /// Возвращает сотрудника для назначения ответственным за заявку.
        /// </remarks>
        [HttpGet]
        [RoleAuthorized(2, 3)]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _dictionariesService.GetAllUsersAsync();
        }
    }
}
