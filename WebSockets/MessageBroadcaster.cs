using project2025.Service;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text.Json;
using System.Text;

namespace project2025.Middleware;

public class MessageBroadcaster
{
    public static ConcurrentDictionary<Guid, WebSocket> Clients = new();

    public static async Task BroadcastUpdatedMessages(int groupId, IServiceProvider services)
    {
        using var scope = services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<Iservice>();

        var messages = service.GetLearnData(groupId);
        var json = JsonSerializer.Serialize(messages);
        var bytes = Encoding.UTF8.GetBytes(json);

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
}
