using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

public class SseHandler
{
    public async Task StreamEventsAsync(HttpContext context, CancellationToken cancellationToken)
    {
        context.Response.ContentType = "text/event-stream";
        context.Response.Headers.Add("Cache-Control", "no-cache");
        context.Response.Headers.Add("Connection", "keep-alive");

        var writer = new StreamWriter(context.Response.Body);
        var count = 0;

        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                await writer.WriteLineAsync($"data: Message {count++} at {DateTime.Now}\n");
                await writer.FlushAsync();
                await Task.Delay(1000, cancellationToken);
            }
        }
        catch (OperationCanceledException)
        {
            // Handle the cancellation gracefully
        }
        finally
        {
            writer.Dispose();
        }
    }
}
