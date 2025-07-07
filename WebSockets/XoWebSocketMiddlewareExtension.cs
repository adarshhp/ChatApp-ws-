using Microsoft.AspNetCore.Builder;

namespace project2025.Middleware
{
    public static class XOWebSocketMiddlewareExtensions
    {
        public static IApplicationBuilder XoUseWebSocketHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<XoWebSocketMiddleware>();
        }
    }
}
