using EduRequestSystemAPI.DatabaseContext;
using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using EduRequestSystemAPI.UniversalMethods;
using Microsoft.AspNetCore.Mvc;

namespace EduRequestSystemAPI.Services
{
    public class RequestService : IRequestService
    {
        private readonly ContextDb _context;

        public RequestService(ContextDb context)
        {
            _context = context;
        }

        public Task<IActionResult> AssignManagerAsync(int requestId, int managerId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> ChangeStatusAsync(int requestId, int newStatusId, int currentUserId)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> CreateRequestAsync(CreateRequest createRequestModel)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> FilterRequestsAsync(int? statusId, int? directionId, string? searchTerm)
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetAllRequestsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<IActionResult> GetRequestByIdAsync(int requestId)
        {
            throw new NotImplementedException();
        }
    }
}
