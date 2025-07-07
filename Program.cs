using Microsoft.EntityFrameworkCore;
using project2025.DBContexts;
using project2025.Models;
using project2025.Repository.Repositories;
using project2025.Repository;
using project2025.Service.Services;
using project2025.Service;
using project2025.Middleware;
using System.Text.Json;
using System.Text;

// Create builder
var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddScoped<Irepository, userrepo>();
builder.Services.AddTransient<Iservice, userservice>();
builder.Services.AddSingleton<SseService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp", policy =>
    {
        policy.WithOrigins("http://localhost:9000", "http://192.168.2.84:9000")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

var app = builder.Build();

// Swagger (for development)
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();
app.UseCors("AllowReactApp");
app.UseAuthorization();

app.UseWebSockets();
app.XoUseWebSocketHandler(); // Assuming this is your middleware for WebSocket

// SSE Endpoint
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();

    endpoints.MapGet("/sse", async context =>
    {
        var sseService = context.RequestServices.GetRequiredService<SseService>();

        context.Response.Headers.Add("Content-Type", "text/event-stream");
        context.Response.Headers.Add("Cache-Control", "no-cache");
        context.Response.Headers.Add("Connection", "keep-alive");

        var client = new StreamWriter(context.Response.Body, Encoding.UTF8, leaveOpen: true);
        sseService.AddClient(client);

        // Keep-alive to prevent disconnects
        while (!context.RequestAborted.IsCancellationRequested)
        {
            await Task.Delay(10000);
            await client.WriteLineAsync(": keep-alive\n");
            await client.FlushAsync();
        }
    });

    // Optional: Broadcast test endpoint
    endpoints.MapPost("/broadcast", async context =>
    {
        var sseService = context.RequestServices.GetRequiredService<SseService>();
        using var reader = new StreamReader(context.Request.Body);
        var body = await reader.ReadToEndAsync();

        try
        {
            var group = JsonSerializer.Deserialize<Group>(body);
            if (group == null)
            {
                context.Response.StatusCode = 400;
                await context.Response.WriteAsync("Invalid Group payload");
                return;
            }

            await sseService.BroadcastAsync(group);
            context.Response.StatusCode = 200;
            await context.Response.WriteAsync("Broadcast sent");
        }
        catch (JsonException)
        {
            context.Response.StatusCode = 400;
            await context.Response.WriteAsync("Invalid JSON format");
        }
    });

});

app.UseHttpsRedirection();
app.Run();
