using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using project2025.Service;

namespace project2025.Middleware;

public class WebSocketBroadcaster
{
    public static ConcurrentDictionary<Guid, WebSocket> Clients = new();

    public static async Task BroadcastAsync(int type, object payload, IServiceProvider services)
    {
        string json = JsonSerializer.Serialize(new { type, data = payload });
        byte[] bytes = Encoding.UTF8.GetBytes(json);

        foreach (var client in Clients.Values)
        {
            if (client.State == WebSocketState.Open)
            {
                await client.SendAsync(
                    new ArraySegment<byte>(bytes),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None
                );
            }
        }
    }

    public static async Task BroadcastUpdatedMessages(int groupId, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<Iservice>();
        var messages = service.GetLearnData(groupId);
        await BroadcastAsync(1, messages, services);
    }

    public static async Task BroadcastUpdatedXo(IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<Iservice>();
        var xoData = service.xogame_Tables();
        await BroadcastAsync(2, xoData, services);
    }
}