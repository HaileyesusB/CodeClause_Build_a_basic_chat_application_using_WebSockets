using CodeClause_WebSocket.Handlers;
using CodeClause_WebSocket.SocketManager;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.DependencyInjection;

namespace CodeClause_WebSocket
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddRazorPages();
            builder.Services.AddWebSocketManager();
            var serviceProvider = builder.Services.BuildServiceProvider();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
              //  app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
              //  app.UseHsts();
            }

           // app.UseHttpsRedirection();
           // app.UseStaticFiles();

           // app.UseRouting();

          //  app.UseAuthorization();

           // app.MapRazorPages();
            app.UseWebSockets();
            app.MapSocket("/ws", serviceProvider.GetService<WebSocketMessageHandler>());
            app.UseStaticFiles();

           // app.Run();
        }
    }
}