using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface IUserService
    {
        // вход и регистрация
        Task<IActionResult> RegUserAsync(RegUserModel newUser);
        Task<IActionResult> AuthUserAsync(AuthUserModel loginData);

        // управление пользователями (для админа)
        Task<IActionResult> GetUserByIdAsync(int userId);
        Task<IActionResult> GetAllUsersAsync();
        Task<IActionResult> ChangeUserRoleAsync(int userId, int newRoleId);
    }
}
