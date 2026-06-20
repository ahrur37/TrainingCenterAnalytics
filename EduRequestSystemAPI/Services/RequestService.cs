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
    public class RequestService : IRequestService
    {
        private readonly ContextDb _context;

        public RequestService(ContextDb context)
        {
            _context = context;
        }

        public async Task<IActionResult> AssignManagerAsync(int requestId, int managerId)
        {
            try
            {
                var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

                request.AssigneeId = managerId;
                request.UpdatedAt = DateTime.UtcNow;

                if (request.StatusId == 1)
                {
                    request.StatusId = 2;

                    var statusHistory = new StatusHistory
                    {
                        CreatedAt = DateTime.UtcNow,
                        OldStatusId = 1,
                        NewStatusId = 2,
                        RequestId = requestId,
                        UserId = managerId
                    };

                    _context.StatusHistories.Add(statusHistory);
                }

                _context.Requests.Update(request);

                var auditLog = new AuditLog
                {
                    Action = AuditAction.UpdateRequest,
                    EntityName = AuditEntity.Request,
                    EntityId = requestId,
                    UserId = managerId,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Заявке ID {requestId} назначен менеджер ID {managerId}. Статус автоматически изменен с 'Новая' на 'В работе'."
                };

                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();

                return new OkObjectResult("Менеджер успешно назначен на заявку");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при назначении менеджера: {ex.Message}");
            }
        }

        public async Task<IActionResult> ChangeStatusAsync(int requestId, int newStatusId, int currentUserId, int roleId)
        {
            try
            {
                var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);

                if (roleId != 3 && roleId != 4)
                {
                    if (request.AssigneeId != currentUserId)
                        return new BadRequestObjectResult("Вы не можете изменить статус чужой заявки. Вы не являетесь ответственным менеджером.");
                }

                int oldStatusId = request.StatusId;
                request.StatusId = newStatusId;
                request.UpdatedAt = DateTime.UtcNow;

                var statusHistory = new StatusHistory
                {
                    CreatedAt = DateTime.UtcNow,
                    OldStatusId = oldStatusId,
                    NewStatusId = newStatusId,
                    RequestId = requestId,
                    UserId = currentUserId 
                };
                _context.StatusHistories.Add(statusHistory);

                var auditLog = new AuditLog
                {
                    Action = AuditAction.ChangeStatus,
                    EntityName = AuditEntity.Request,
                    EntityId = requestId,
                    UserId = currentUserId,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Статус заявки ID {requestId} изменен с {oldStatusId} на {newStatusId} пользователем ID {currentUserId}"
                };
                _context.AuditLogs.Add(auditLog);

                _context.Requests.Update(request);
                await _context.SaveChangesAsync();

                return new OkObjectResult("Статус заявки успешно изменен");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при изменении статуса: {ex.Message}");
            }
        }

        public async Task<IActionResult> CreateRequestAsync(CreateRequest createRequestModel)
        {
            try
            {
                var newRequest = new Request
                {
                    Topic = createRequestModel.Topic,
                    Description = createRequestModel.Description,
                    ContactInfo = createRequestModel.ContactInfo,
                    DirectionId = createRequestModel.DirectionId,
                    TrainingFormatId = createRequestModel.TrainingFormatId,
                    StatusId = 1,
                    CreatedAt = DateTime.UtcNow,
                    AuthorId = createRequestModel.AuthorId
                };

                _context.Requests.Add(newRequest);
                await _context.SaveChangesAsync();

                var auditLog = new AuditLog
                {
                    Action = AuditAction.CreateRequest,
                    EntityName = AuditEntity.Request,
                    EntityId = newRequest.Id,
                    UserId = createRequestModel.AuthorId,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Пользователь создал заявку '{newRequest.Topic}'"
                };

                _context.AuditLogs.Add(auditLog);
                await _context.SaveChangesAsync();

                return new OkObjectResult(new 
                { 
                    message = "Заявка успешно создана", 
                    requestId = newRequest.Id 
                });
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при создании заявки: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetRequestsAsync(int? statusId, int? directionId, string? searchTerm, int? assigneeId, int? authorId)
        {
            try
            {
                var query = _context.Requests
                    .Include(r => r.Direction)
                    .Include(r => r.TrainingFormat)
                    .Include(r => r.Status)
                    .Include(r => r.Author)
                    .Include(r => r.Assignee)
                    .AsQueryable();

                if (statusId.HasValue)
                {
                    query = query.Where(r => r.StatusId == statusId.Value);
                }

                if (directionId.HasValue)
                {
                    query = query.Where(r => r.DirectionId == directionId.Value);
                }

                if (assigneeId.HasValue)
                {
                    query = query.Where(r => r.AssigneeId == assigneeId.Value || r.StatusId == 1);
                }

                if (authorId.HasValue)
                {
                    query = query.Where(r => r.AuthorId == authorId.Value);
                }

                if (!string.IsNullOrWhiteSpace(searchTerm))
                {
                    searchTerm = searchTerm.Trim().ToLower();
                    query = query.Where(r => r.Topic.ToLower().Contains(searchTerm) ||
                                             r.Description.ToLower().Contains(searchTerm));
                }

                var filteredRequests = await query.OrderBy(r => r.Id).ToListAsync();
                return new OkObjectResult(filteredRequests);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при фильтрации заявок: {ex.Message}");
            }
        }

        public async Task<IActionResult> UpdateRequestAsync(int requestId, int currentUserId, int roleId, UpdateRequest model)
        {
            try
            {
                var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                    return new NotFoundObjectResult("Заявка не найдена.");

                if (roleId == 1)
                {
                    if (request.AuthorId != currentUserId)
                        return new BadRequestObjectResult("Вы можете редактировать только свои заявки.");
                    if (request.StatusId != 1)
                        return new BadRequestObjectResult("Редактировать можно только заявки со статусом 'Новая'.");
                }
                else if (roleId == 2)
                {
                    if (request.AssigneeId != currentUserId)
                        return new BadRequestObjectResult("Вы можете редактировать только назначенные вам заявки.");
                }

                request.Topic = model.Topic;
                request.Description = model.Description;
                request.ContactInfo = model.ContactInfo;
                request.DirectionId = model.DirectionId;
                request.TrainingFormatId = model.TrainingFormatId;
                request.UpdatedAt = DateTime.UtcNow;

                var auditLog = new AuditLog
                {
                    Action = AuditAction.UpdateRequest,
                    EntityName = AuditEntity.Request,
                    EntityId = requestId,
                    UserId = currentUserId,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Пользователь ID {currentUserId} отредактировал заявку ID {requestId}"
                };
                _context.AuditLogs.Add(auditLog);

                await _context.SaveChangesAsync();
                return new OkObjectResult("Заявка успешно обновлена.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при обновлении заявки: {ex.Message}");
            }
        }

        public async Task<IActionResult> DeleteRequestAsync(int requestId, int currentUserId, int roleId)
        {
            try
            {
                var request = await _context.Requests.FirstOrDefaultAsync(r => r.Id == requestId);
                if (request == null)
                    return new NotFoundObjectResult("Заявка не найдена.");

                if (roleId == 1)
                {
                    if (request.AuthorId != currentUserId)
                        return new BadRequestObjectResult("Вы можете удалять только свои заявки.");
                    if (request.StatusId != 1)
                        return new BadRequestObjectResult("Удалить можно только заявку со статусом 'Новая'.");
                }
                else if (roleId == 2)
                {
                    if (request.AssigneeId != currentUserId)
                        return new BadRequestObjectResult("Вы можете удалять только назначенные вам заявки.");
                }

                var auditLog = new AuditLog
                {
                    Action = AuditAction.UpdateRequest,
                    EntityName = AuditEntity.Request,
                    EntityId = requestId,
                    UserId = currentUserId,
                    Timestamp = DateTime.UtcNow,
                    Details = $"Пользователь ID {currentUserId} удалил заявку ID {requestId}"
                };
                _context.AuditLogs.Add(auditLog);

                _context.Requests.Remove(request);
                await _context.SaveChangesAsync();
                return new OkObjectResult("Заявка успешно удалена.");
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при удалении заявки: {ex.Message}");
            }
        }

        public async Task<IActionResult> GetRequestByIdAsync(int requestId)
        {
            try
            {
                var request = await _context.Requests
                    .Include(r => r.Direction)
                    .Include(r => r.TrainingFormat)
                    .Include(r => r.Status)
                    .Include(r => r.Author)
                    .Include(r => r.Assignee)
                    .FirstOrDefaultAsync(r => r.Id == requestId);

                return new OkObjectResult(request);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult($"Ошибка при получении заявки: {ex.Message}");
            }
        }
    }
}
