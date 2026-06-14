using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduRequestSystemAPI.CustomAttributes;

namespace EduRequestSystemAPI.Controllers
{
    [RoleAuthorized(1, 2, 3, 4)]
    public class DictionariesController : Controller
    {
        private readonly IDictionariesService _dictionariesService;

        public DictionariesController(IDictionariesService dictionariesService)
        {
            _dictionariesService = dictionariesService;
        }

        [HttpGet]
        [Route("GetDirections")]
        public async Task<IActionResult> GetDirections()
        {
            return await _dictionariesService.GetDirectionsAsync();
        }

        [HttpGet]
        [Route("GetTrainingFormats")]
        public async Task<IActionResult> GetTrainingFormats()
        {
            return await _dictionariesService.GetTrainingFormatsAsync();
        }

        [HttpGet]
        [Route("GetStatuses")]
        public async Task<IActionResult> GetStatuses()
        {
            return await _dictionariesService.GetStatusesAsync();
        }

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            return await _dictionariesService.GetRolesAsync();
        }

        [HttpGet]
        [RoleAuthorized(2, 3)]
        [Route("GetAllUsers")]
        public async Task<IActionResult> GetAllUsers()
        {
            return await _dictionariesService.GetAllUsersAsync();
        }
    }
}
