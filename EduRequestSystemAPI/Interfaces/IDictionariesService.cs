using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface IDictionariesService
    {
        Task<IActionResult> GetDirectionsAsync(); // направления
        Task<IActionResult> CreateDirectionAsync(DictionaryModel model);
        Task<IActionResult> UpdateDirectionAsync(int id, DictionaryModel model);
        Task<IActionResult> DeleteDirectionAsync(int id);
        
        Task<IActionResult> GetTrainingFormatsAsync(); // форматы обучения
        Task<IActionResult> CreateTrainingFormatAsync(DictionaryModel model);
        Task<IActionResult> UpdateTrainingFormatAsync(int id, DictionaryModel model);
        Task<IActionResult> DeleteTrainingFormatAsync(int id);
        
        Task<IActionResult> GetStatusesAsync(); // статусы
        Task<IActionResult> CreateStatusAsync(DictionaryModel model);
        Task<IActionResult> UpdateStatusAsync(int id, DictionaryModel model);
        Task<IActionResult> DeleteStatusAsync(int id);
        
        Task<IActionResult> GetRolesAsync(); // роли
        Task<IActionResult> CreateRoleAsync(DictionaryModel model);
        Task<IActionResult> UpdateRoleAsync(int id, DictionaryModel model);
        Task<IActionResult> DeleteRoleAsync(int id);

        Task<IActionResult> GetAllUsersAsync(); // пользователи
    }
}
