using System.Net.Http.Headers;

namespace FrontendApp.Middleware
{
    public class MiddlewareJWT
    {
        private readonly RequestDelegate _next;

        public MiddlewareJWT(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
           
            if (context.Request.Path.StartsWithSegments("/Login"))
            {
                await _next(context);
                return;
            }

            
            var token = context.Session.GetString("JWToken");

            if (!string.IsNullOrEmpty(token))
            {
            
                context.Request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token).ToString();
            }

            await _next(context);
        }
    }
}
