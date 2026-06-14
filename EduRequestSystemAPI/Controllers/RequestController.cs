using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduRequestSystemAPI.CustomAttributes;

namespace EduRequestSystemAPI.Controllers
{
    public class RequestController : Controller
    {
        private readonly IRequestService _requestService;

        public RequestController(IRequestService requestService)
        {
            _requestService = requestService;
        }

        [HttpPost]
        [RoleAuthorized(1)]
        [Route("CreateRequest")]
        public async Task<IActionResult> CreateRequest([FromBody] CreateRequest createRequestModel)
        {
            return await _requestService.CreateRequestAsync(createRequestModel);
        }

        [HttpGet]
        [RoleAuthorized(1, 2, 3, 4)]
        [Route("GetRequestById/{requestId}")]
        public async Task<IActionResult> GetRequestById(int requestId)
        {
            return await _requestService.GetRequestByIdAsync(requestId);
        }

        [HttpGet]
        [RoleAuthorized(1, 2, 3, 4)]
        [Route("GetRequests")]
        public async Task<IActionResult> GetRequests(int? statusId, int? directionId, string? searchTerm, int? assigneeId, int? authorId)
        {
            return await _requestService.GetRequestsAsync(statusId, directionId, searchTerm, assigneeId, authorId);
        }

        [HttpPost]
        [RoleAuthorized(2, 3)]
        [Route("AssignManager")]
        public async Task<IActionResult> AssignManager(int requestId, int managerId)
        {
            return await _requestService.AssignManagerAsync(requestId, managerId);
        }

        [HttpPost]
        [RoleAuthorized(2, 3)]
        [Route("ChangeStatus")]
        public async Task<IActionResult> ChangeStatus(int requestId, int newStatusId, int currentUserId)
        {
            return await _requestService.ChangeStatusAsync(requestId, newStatusId, currentUserId);
        }
    }
}
