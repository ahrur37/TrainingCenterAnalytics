using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface IDictionariesService
    {
        Task<IActionResult> GetDirectionsAsync();      // список всех направлений
        Task<IActionResult> GetTrainingFormatsAsync(); // список всех форматов обучения
        Task<IActionResult> GetStatusesAsync();        // список всех возможных статусов
    }
}
