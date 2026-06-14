using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.Design;

namespace EduRequestSystemAPI.Services
{
    public class DictionariesService : IDictionariesService
    {
        private readonly ContextDb _context;

        public DictionariesService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> GetDirectionsAsync()
        {
            try
            {
                var directions = await _context.Directions.ToListAsync();
                return new OkObjectResult(directions);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении списка направлений: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetStatusesAsync()
        {
            try
            {
                var statuses = await _context.Statuses.ToListAsync();
                return new OkObjectResult(statuses);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении списка статусов: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetTrainingFormatsAsync()
        {
            try
            {
                var formats = await _context.TrainingFormats.ToListAsync();
                return new OkObjectResult(formats);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении списка форматов обучения: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetRolesAsync()
        {
            try
            {
                var roles = await _context.Roles.ToListAsync();
                return new OkObjectResult(roles);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении списка ролей: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetAllUsersAsync()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                return new OkObjectResult(users);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении списка пользователей: {ex.Message}");
            }
        }
    }
}
