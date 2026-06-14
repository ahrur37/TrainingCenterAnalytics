using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EduRequestSystemAPI.CustomAttributes;

namespace EduRequestSystemAPI.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("RegUser")]
        public async Task<IActionResult> RegUser([FromBody] RegUserModel newUser)
        {
            return await _userService.RegUserAsync(newUser);
        }

        [HttpPost]
        [Route("AuthUser")]
        public async Task<IActionResult> AuthUser([FromBody] AuthUserModel loginData)
        {
            return await _userService.AuthUserAsync(loginData);
        }

        [HttpGet]
        [RoleAuthorized(2, 3, 4)]
        [Route("GetUserById/{userId}")]
        public async Task<IActionResult> GetUserById(int userId)
        {
            return await _userService.GetUserByIdAsync(userId);
        }

        [HttpPut]
        [RoleAuthorized(3)]
        [Route("ChangeUserRole")]
        public async Task<IActionResult> ChangeUserRole(int userId, int newRoleId)
        {
            return await _userService.ChangeUserRoleAsync(userId, newRoleId);
        }
    }
}
