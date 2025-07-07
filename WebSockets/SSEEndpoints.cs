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
