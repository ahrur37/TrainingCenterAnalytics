using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface IRequestService
    {
        Task<IActionResult> CreateRequestAsync(CreateRequest createRequestModel); // создать заявку
        Task<IActionResult> GetRequestsAsync(int? statusId, int? directionId, string? searchTerm, int? assigneeId, int? authorId); // получить заявки с фильтрами
        Task<IActionResult> GetRequestByIdAsync(int requestId); // получить все заявки от конкретного заявителя
        Task<IActionResult> AssignManagerAsync(int requestId, int managerId); // взять заявку в работу
        Task<IActionResult> ChangeStatusAsync(int requestId, int newStatusId, int currentUserId); // изменить статус заявки
    }
}
