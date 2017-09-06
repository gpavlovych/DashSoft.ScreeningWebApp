using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Screening.WebAPI.Core.Data;

namespace Screening.WebAPI.Core.Authorization
{
    public class TeacherAuthorizationHandler : AuthorizationHandler<TeacherAuthorizationRequirement>
    {
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly UserManager<User> userManager;

        public TeacherAuthorizationHandler(IHttpContextAccessor httpContextAccessor, UserManager<User> userManager)
        {
            if (httpContextAccessor == null)
                throw new ArgumentNullException(nameof(httpContextAccessor));

            if (userManager == null)
                throw new ArgumentNullException(nameof(userManager));

            this.userManager = userManager;
            this.httpContextAccessor = httpContextAccessor;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            TeacherAuthorizationRequirement authorizationRequirement)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));

            var currentUserPrincipal = httpContextAccessor.HttpContext.User;
            var currentUser = await userManager.GetUserAsync(currentUserPrincipal);
            var roles = await userManager.GetRolesAsync(currentUser);
            if (roles.Contains("teacher"))
                context.Succeed(authorizationRequirement);
            else
                context.Fail();
        }
    }
}
