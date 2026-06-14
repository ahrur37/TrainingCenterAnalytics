using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface IDictionariesService
    {
        Task<IActionResult> GetDirectionsAsync(); // направления
        Task<IActionResult> GetTrainingFormatsAsync(); // форматы обучения
        Task<IActionResult> GetStatusesAsync(); // статусы
        Task<IActionResult> GetRolesAsync(); // роли
        Task<IActionResult> GetAllUsersAsync(); // пользователи
    }
}
