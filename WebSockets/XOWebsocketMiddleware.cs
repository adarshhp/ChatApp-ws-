using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using project2025.Models;
using project2025.Models.Responces;
using project2025.Models.Responses;
using project2025.Service;

namespace project2025.Middleware
{
    public class XoWebSocketMiddleware
    {
        private readonly RequestDelegate _next;

        public XoWebSocketMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, Iservice service)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.WebSockets.IsWebSocketRequest)
                {
                    var socket = await context.WebSockets.AcceptWebSocketAsync();
                    var clientId = Guid.NewGuid();
                    WebSocketBroadcaster.Clients.TryAdd(clientId, socket);
                  //  MessageBroadcaster.Clients.TryAdd(clientId, socket);

                    var buffer = new byte[1024 * 4];

                    try
                    {
                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                        while (!result.CloseStatus.HasValue)
                        {
                            var rawMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                            Console.WriteLine("📩 Received: " + rawMessage);

                            var DEserializedData = WSDataDeserializer.FromJson(rawMessage);
                            if (DEserializedData != null)
                            {
                                switch (DEserializedData.type)
                                {
                                    case 1:
                                        var learnPost = DEserializedData.data as LearnPost;
                                        if (learnPost != null)
                                        {
                                            service.PostLearnData(learnPost);
                                            await WebSocketBroadcaster.BroadcastUpdatedMessages(learnPost.group_id, context.RequestServices);
                                        }
                                        break;

                                    case 2:
                                        var xoGame = DEserializedData.data as AddXO;
                                        if (xoGame != null)
                                        {
                                            service.AddXo(xoGame);
                                            await WebSocketBroadcaster.BroadcastUpdatedXo(context.RequestServices);
                                        }
                                        break;

                                    default:
                                        Console.WriteLine("⚠️ Unknown type received");
                                        break;
                                }
                            }
                            result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("❌ WebSocket error: " + ex.Message);
                    }
                    finally
                    {
                        if (WebSocketBroadcaster.Clients.TryRemove(clientId, out var closedSocket))
                        {
                            if (closedSocket.State == WebSocketState.Open)
                            {
                                await closedSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnected", CancellationToken.None);
                            }
                            Console.WriteLine("🔌 WebSocket client disconnected");
                        }
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
