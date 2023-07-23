using Microsoft.AspNetCore.WebSockets;
using System.Reflection;

namespace CodeClause_WebSocket.SocketManager
{
    public static class SocketExention
    {
        public static IServiceCollection AddWebSocketManager(this IServiceCollection services) 
        {
            services.AddTransient<ConnectionManager>();
            foreach(var type in Assembly.GetEntryAssembly().ExportedTypes)
            {
                if(type.GetTypeInfo().BaseType == typeof(SocketHandler)) 
                {
                  services.AddSingleton(type);
                }
                
            }
            return services;
        }

        public static IApplicationBuilder MapSocket(this IApplicationBuilder app, PathString path, 
            SocketHandler socket) 
        {
           return app.Map(path,(x) => x.UseMiddleware<SocketMiddleware>(socket));
        
        }
    }
}
