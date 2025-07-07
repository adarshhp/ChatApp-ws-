using System.Collections.Concurrent;
using System.IO;
using System.Threading.Tasks;
using project2025.Models;
using System.Text.Json;

public class SseService
{
    private readonly ConcurrentBag<StreamWriter> _clients = new();
    public void AddClient(StreamWriter writer)
    {
        _clients.Add(writer);
    }
    public async Task BroadcastAsync(Group group)
    {
        string message = JsonSerializer.Serialize(group);
        foreach (var client in _clients)
        {
            try
            {
                await client.WriteLineAsync($"data: {message}\n");
                await client.FlushAsync();
            }
            catch
            {
                // Handle broken connections if needed
            }
        }
    }
}
