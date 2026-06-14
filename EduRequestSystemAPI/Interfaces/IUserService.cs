using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface IUserService
    {
        Task<IActionResult> RegUserAsync(RegUserModel newUser); // регистрация
        Task<IActionResult> AuthUserAsync(AuthUserModel loginData); // вход
        Task<IActionResult> GetUserByIdAsync(int userId); // получить конкретного пользователя
        Task<IActionResult> ChangeUserRoleAsync(int userId, int newRoleId); // изменить роль пользователя
    }
}
