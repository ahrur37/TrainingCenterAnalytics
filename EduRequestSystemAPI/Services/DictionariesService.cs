using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
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

        public async Task<IActionResult> CreateDirectionAsync(DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название направления не может быть пустым.");

                if (await _context.Directions.AnyAsync(d => d.Name == model.Name))
                    return new BadRequestObjectResult("Направление с таким названием уже существует.");

                var direction = new Direction { Name = model.Name };
                _context.Directions.Add(direction);
                await _context.SaveChangesAsync();

                return new OkObjectResult(direction);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при создании направления: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateDirectionAsync(int id, DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название направления не может быть пустым.");

                var direction = await _context.Directions.FindAsync(id);

                direction.Name = model.Name;
                await _context.SaveChangesAsync();

                return new OkObjectResult(direction);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при обновлении направления: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteDirectionAsync(int id)
        {
            try
            {
                var direction = await _context.Directions.FindAsync(id);

                direction.IsActive = false;
                await _context.SaveChangesAsync();

                return new OkObjectResult($"Направление '{direction.Name}' успешно удалено.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при удалении направления (возможно, оно используется в заявках): {ex.Message}");
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

        public async Task<IActionResult> CreateStatusAsync(DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название статуса не может быть пустым.");

                if (await _context.Statuses.AnyAsync(s => s.Name == model.Name))
                    return new BadRequestObjectResult("Статус с таким названием уже существует.");

                var status = new Status { Name = model.Name };
                _context.Statuses.Add(status);
                await _context.SaveChangesAsync();

                return new OkObjectResult(status);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при создании статуса: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateStatusAsync(int id, DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название статуса не может быть пустым.");

                var status = await _context.Statuses.FindAsync(id);

                status.Name = model.Name;
                await _context.SaveChangesAsync();

                return new OkObjectResult(status);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при обновлении статуса: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteStatusAsync(int id)
        {
            try
            {
                var status = await _context.Statuses.FindAsync(id);

                status.IsActive = false;
                await _context.SaveChangesAsync();

                return new OkObjectResult($"Статус '{status.Name}' успешно удален.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при удалении статуса: {ex.Message}");
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

        public async Task<IActionResult> CreateTrainingFormatAsync(DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название формата обучения не может быть пустым.");

                if (await _context.TrainingFormats.AnyAsync(f => f.Name == model.Name))
                    return new BadRequestObjectResult("Формат с таким названием уже существует.");

                var format = new TrainingFormat { Name = model.Name };
                _context.TrainingFormats.Add(format);
                await _context.SaveChangesAsync();

                return new OkObjectResult(format);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при создании формата обучения: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateTrainingFormatAsync(int id, DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название формата обучения не может быть пустым.");

                var format = await _context.TrainingFormats.FindAsync(id);

                format.Name = model.Name;
                await _context.SaveChangesAsync();

                return new OkObjectResult(format);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при обновлении формата обучения: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteTrainingFormatAsync(int id)
        {
            try
            {
                var format = await _context.TrainingFormats.FindAsync(id);

                format.IsActive = false;
                await _context.SaveChangesAsync();

                return new OkObjectResult($"Формат '{format.Name}' успешно удален.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при удалении формата обучения: {ex.Message}");
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

        public async Task<IActionResult> CreateRoleAsync(DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название роли не может быть пустым.");

                var role = new Role { Name = model.Name };
                _context.Roles.Add(role);
                await _context.SaveChangesAsync();

                return new OkObjectResult(role);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при создании роли: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateRoleAsync(int id, DictionaryModel model)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(model.Name))
                    return new BadRequestObjectResult("Название роли не может быть пустым.");

                var role = await _context.Roles.FindAsync(id);

                role.Name = model.Name;
                await _context.SaveChangesAsync();

                return new OkObjectResult(role);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при обновлении роли: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteRoleAsync(int id)
        {
            try
            {
                var role = await _context.Roles.FindAsync(id);

                _context.Roles.Remove(role);
                await _context.SaveChangesAsync();

                return new OkObjectResult($"Роль '{role.Name}' успешно удалена.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при удалении роли: {ex.Message}");
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
