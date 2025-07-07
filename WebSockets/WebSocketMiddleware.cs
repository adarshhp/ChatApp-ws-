//using System.Net.WebSockets;
//using System.Text;
//using System.Text.Json;
//using project2025.Models;
//using project2025.Service;

//namespace project2025.Middleware
//{
//    public class WebSocketMiddleware
//    {
//        private readonly RequestDelegate _next;

//        public WebSocketMiddleware(RequestDelegate next)
//        {
//            _next = next;
//        }

//        public async Task InvokeAsync(HttpContext context, Iservice service)
//        {
//            if (context.Request.Path == "/ws")
//            {
//                if (context.WebSockets.IsWebSocketRequest)
//                {
//                    var socket = await context.WebSockets.AcceptWebSocketAsync();
//                    var clientId = Guid.NewGuid();
//                    MessageBroadcaster.Clients.TryAdd(clientId, socket);

//                    var buffer = new byte[1024 * 4];

//                    try
//                    {
//                        var result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

//                        while (!result.CloseStatus.HasValue)
//                        {
//                            var rawMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
//                            Console.WriteLine("📩 Received: " + rawMessage);

//                            var learnPost = JsonSerializer.Deserialize<LearnPost>(rawMessage);
//                            if (learnPost != null)
//                            {
//                                service.PostLearnData(learnPost);
//                                await MessageBroadcaster.BroadcastUpdatedMessages(learnPost.group_id, context.RequestServices);
//                            }

//                            result = await socket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
//                        }
//                    }
//                    catch (Exception ex)
//                    {
//                        Console.WriteLine("❌ WebSocket error: " + ex.Message);
//                    }
//                    finally
//                    {
//                        if (MessageBroadcaster.Clients.TryRemove(clientId, out var closedSocket))
//                        {
//                            if (closedSocket.State == WebSocketState.Open)
//                            {
//                                await closedSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Client disconnected", CancellationToken.None);
//                            }
//                            Console.WriteLine("🔌 WebSocket client disconnected");
//                        }
//                    }
//                }
//                else
//                {
//                    context.Response.StatusCode = 400;
//                }
//            }
//            else
//            {
//                await _next(context);
//            }
//        }
//    }
//}














//adding sse in api 
//using Microsoft.AspNetCore.Mvc;
//using SseExample.Services;

//namespace SseExample.Controllers
//{
//    [ApiController]
//    [Route("api/events")]
//    public class EventController : ControllerBase
//    {
//        private readonly SseService _sseService;

//        public EventController(SseService sseService)
//        {
//            _sseService = sseService;
//        }

//        [HttpPost]
//        public async Task<IActionResult> PostEvent([FromBody] string message)
//        {
//            await _sseService.BroadcastAsync(message);
//            return Ok("Event broadcasted");
//        }
//    }
//}
