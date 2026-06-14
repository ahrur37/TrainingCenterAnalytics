using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Enums;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Models;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.UniversalMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EduRequestSystemAPI.Services
{
    public class UserService : IUserService
    {
        private readonly ContextDb _context;

        private readonly jwtGenerator _jwtGenerator;

        public UserService(ContextDb context, jwtGenerator jwtGenerator)
        {
            _context = context;
            _jwtGenerator = jwtGenerator;
        }

        public async Task<IActionResult> AuthUserAsync(AuthUserModel loginData)
        {
            try
            {
                var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == loginData.Email && u.Password == loginData.Password);

                if (user == null)
                {
                    return new NotFoundObjectResult(new
                    {
                        status = false,
                        message = "Пользователь не найден."
                    });
                }

                string token = _jwtGenerator.GenerateToken(new LoginPassword()
                {
                    UserId = user.Id,
                    RoleId = user.RoleId
                });

                await _context.Sessions.AddAsync(new Session
                {
                    Token = token,
                    UserId = user.Id
                });

                await _context.AuditLogs.AddAsync(new AuditLog
                {
                    Action = AuditAction.Login,
                    EntityName = AuditEntity.User,
                    EntityId = user.Id,
                    UserId = user.Id,
                    Details = $"Пользователь {user.Email} успешно вошел в систему.",
                    Timestamp = DateTime.UtcNow
                });

                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true,
                    roleId = user.RoleId,
                    token
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при авторизации: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetUserByIdAsync(int userId)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.Id == userId);

                if (user == null)
                {
                    return new NotFoundObjectResult($"Пользователь с ID {userId} не найден");
                }

                return new OkObjectResult(user);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении данных пользователя: {ex.Message}");
            }
        }

        public async Task<IActionResult> ChangeUserRoleAsync(int userId, int newRoleId)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

                var roleExists = await _context.Roles.AnyAsync(r => r.Id == newRoleId);

                int oldRoleId = user.RoleId;
                user.RoleId = newRoleId;
                _context.Users.Update(user);

                var auditLog = new AuditLog
                {
                    Action = AuditAction.ChangeRole,
                    EntityName = AuditEntity.User,
                    EntityId = userId,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Администратор изменил роль пользователю ID {userId} с роли {oldRoleId} на {newRoleId}"
                };
                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();
                return new OkObjectResult("Роль пользователя успешно изменена");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при смене роли: {ex.Message}");
            }
        }

        public async Task<IActionResult> RegUserAsync(RegUserModel newUser)
        {
            try
            {
                if (string.IsNullOrEmpty(newUser.Name) || string.IsNullOrEmpty(newUser.Email))
                {
                    return new BadRequestObjectResult(new
                    {
                        status = false,
                        message = "Имя и Email не могут быть пустыми"
                    });
                }

                if (newUser.Password.Length < 6)
                {
                    return new BadRequestObjectResult(new
                    {
                        status = false,
                        message = "Пароль должен содержать не менее 6 символов"
                    });
                }

                var existingUser = await _context.Users.FirstOrDefaultAsync(c =>
                    c.Email.ToLower() == newUser.Email.ToLower());

                if (existingUser != null)
                {
                    return new BadRequestObjectResult(new
                    {
                        status = false,
                        message = "Пользователь с таким адресом уже зарегистрирован"
                    });
                }

                var user = new User()
                {
                    Name = newUser.Name,
                    Email = newUser.Email,
                    Password = newUser.Password,
                    RoleId = 1
                };

                var auditLog = new AuditLog
                {
                    Action = AuditAction.Register,
                    EntityName = AuditEntity.User,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Зарегистрирован новый пользователь: {user.Email} (Имя: {user.Name}).",
                    User = user
                };

                await _context.Users.AddAsync(user);
                await _context.AuditLogs.AddAsync(auditLog);
                await _context.SaveChangesAsync();

                auditLog.EntityId = user.Id;
                await _context.SaveChangesAsync();

                return new OkObjectResult(new
                {
                    status = true
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при регистрации: {ex.Message}");
            }
        }
    }
}
