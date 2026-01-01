using Microsoft.AspNetCore.Identity;
using TestWebAPIs.Auth;

namespace TestWebAPIs.Middleware
{
    public class SessionAuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public SessionAuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            var token = context.Session.GetString("AuthToken");
            var userId = context.Session.GetString("UserId");

            if (!string.IsNullOrEmpty(token) && !string.IsNullOrEmpty(userId))
            {
                var user = await userManager.FindByIdAsync(userId);
                if (user != null)
                {
                    var isValid = await userManager.VerifyUserTokenAsync(user, TokenOptions.DefaultProvider, "API", token);
                    if (isValid)
                    {
                        await signInManager.SignInAsync(user, isPersistent: false);
                    }
                }
            }

            await _next(context);
        }
    }
}
