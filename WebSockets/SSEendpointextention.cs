using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace project2025.Middleware
{
    public static class SseEndpointExtensions
    {
        public static void MapSseEndpoint(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/sse", async (HttpContext context, SseService sseService, CancellationToken cancellationToken) =>
            {
                context.Response.ContentType = "text/event-stream";
                context.Response.Headers.Add("Cache-Control", "no-cache");
                context.Response.Headers.Add("Connection", "keep-alive");

                var writer = new StreamWriter(context.Response.Body);
                sseService.AddClient(writer);

                try
                {
                    while (!cancellationToken.IsCancellationRequested)
                    {
                        await Task.Delay(Timeout.Infinite, cancellationToken);
                    }
                }
                catch (OperationCanceledException)
                {
                    // Connection was cancelled
                }
                finally
                {
                    writer.Dispose();
                }
            });
        }
    }

    public static class SseEndpoints
    {
        public static IEndpointRouteBuilder MapSseEndpoints(this IEndpointRouteBuilder endpoints)
        {
            endpoints.MapGet("/sse", async (HttpContext context) =>
            {
                context.Response.Headers.Add("Content-Type", "text/event-stream");

                for (var i = 0; i < 10; i++)
                {
                    await context.Response.WriteAsync($"data: Message {i}\n\n");
                    await context.Response.Body.FlushAsync();
                    await Task.Delay(1000);
                }
            });

            return endpoints;
        }
    }


}
