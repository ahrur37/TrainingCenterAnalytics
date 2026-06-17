using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduRequestSystemAPI.CustomAttributes;

namespace EduRequestSystemAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Регистрация нового пользователя в системе.
        /// </summary>
        /// <remarks>
        /// Ппубличный метод. 
        /// Создает учетную запись нового пользователя. По умолчанию новому аккаунту присваивается роль Заявителя (1).
        /// </remarks>
        /// <param name="newUser">Модель с регистрационными данными пользователя (Почта, Пароль, Имя)</param>
        [HttpPost]
        [Route("RegUser")]
        public async Task<IActionResult> RegUser([FromBody] RegUserModel newUser)
        {
            return await _userService.RegUserAsync(newUser);
        }

        /// <summary>
        /// Вход пользователя в систему.
        /// </summary>
        /// <remarks>
        /// Публичный метод. 
        /// Проверяет логин и пароль. В случае успеха создает сессию и возвращает строку-токен.
        /// </remarks>
        /// <param name="loginData">Модель с учетными данными (Логин и Пароль)</param>
        [HttpPost]
        [Route("AuthUser")]
        public async Task<IActionResult> AuthUser([FromBody] AuthUserModel loginData)
        {
            return await _userService.AuthUserAsync(loginData);
        }

        /// <summary>
        /// Получение информации о пользователе по его уникальному ID.
        /// </summary>
        /// <remarks>
        /// Доступно ролям: Менеджер (2), Администратор (3), Руководитель (4).
        /// Позволяет сотрудникам просматривать профили пользователей.
        /// </remarks>
        /// <param name="userId">Уникальный идентификатор искомого пользователя</param>
        [HttpGet]
        [RoleAuthorized(2, 3, 4)]
        [Route("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            return await _userService.GetUserByIdAsync(userId);
        }

        /// <summary>
        /// Изменение роли пользователя.
        /// </summary>
        /// <remarks>
        /// Доступно строго роли Администратор (3). 
        /// Позволяет выдать пользователю новые права доступа.
        /// </remarks>
        /// <param name="userId">Уникальный идентификатор целевого пользователя</param>
        /// <param name="newRoleId">ID новой роли: 1 - Заявитель, 2 - Менеджер, 3 - Админ, 4 - Руководитель</param>
        [HttpPut]
        [RoleAuthorized(3)]
        [Route("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(int userId, int newRoleId)
        {
            return await _userService.ChangeUserRoleAsync(userId, newRoleId);
        }
    }
}
