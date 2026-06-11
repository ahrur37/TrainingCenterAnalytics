using EduRequestSystemAPI.Interfaces;
using EduRequestSystemAPI.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    }
}
