using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using EduRequestSystemAPI.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace EduRequestSystemAPI.CustomAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class RoleAuthorizedAttribute : Attribute, IAsyncActionFilter
    {
        private readonly int[] _roleId;
        public RoleAuthorizedAttribute(params int[] roleId)
        {
            _roleId = roleId;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var dbContext = context.HttpContext.RequestServices.GetRequiredService<ContextDb>();
            string? token = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();

            if (string.IsNullOrEmpty(token))
            {
                context.Result = new JsonResult(new { error = "no session" }) { StatusCode = 401 };
                return;
            }

            var userSession = await dbContext.Sessions.Include(u => u.User).FirstOrDefaultAsync(u => u.Token == token);
            if (userSession == null)
            {
                context.Result = new JsonResult(new { error = "no session" }) { StatusCode = 401 };
                return;
            }

            if (!_roleId.Contains(userSession.User.RoleId))
            {
                context.Result = new JsonResult(new { error = "No prava" }) { StatusCode = 401 };
                return;
            }

            await next();

        }
    }
}
