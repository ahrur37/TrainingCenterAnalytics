using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Interfaces
{
    public interface IRequestService
    {
        // создание и получение заявок
        Task<IActionResult> CreateRequestAsync(CreateRequest createRequestModel);
        Task<IActionResult> GetAllRequestsAsync();
        Task<IActionResult> GetRequestByIdAsync(int requestId);

        // фильтрация и поиск (для менеджера и руководителя)
        Task<IActionResult> FilterRequestsAsync(int? statusId, int? directionId, string? searchTerm);

        // назначение ответственного менеджера
        Task<IActionResult> AssignManagerAsync(int requestId, int managerId);

        // смена статуса
        Task<IActionResult> ChangeStatusAsync(int requestId, int newStatusId, int currentUserId);
    }
}
