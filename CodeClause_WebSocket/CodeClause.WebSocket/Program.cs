using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CodeClause.WebSocket
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Count() <= 2)
            {
                Process.Start("CodeClause.Websocket.exe");
            }
            StartWebSocket().GetAwaiter().GetResult();
        }

        public static async Task StartWebSocket()
        {
            //var client = new ClientWebSocket();
            try
            {
                using (var clientWebSocket = new ClientWebSocket())
                {
                    // Configure and use the clientWebSocket instance with the proxy settings
                    await clientWebSocket.ConnectAsync(new Uri("ws://localhost:2000/ws"), CancellationToken.None);
                    Console.WriteLine($"Web Socket Connection Established @{DateTime.UtcNow:F}");
                    var send = Task.Run(async () =>
                    {
                        string message;
                        while ((message = Console.ReadLine()) != null && message != string.Empty)
                        {
                            var bytes = Encoding.UTF8.GetBytes(message);
                            await clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
                        }
                        await clientWebSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    });
                    var recieve = RecieveAsync(clientWebSocket);
                    await Task.WhenAll(send, recieve);
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it appropriately
                Console.WriteLine($"WebSocket connection error: {ex.Message}");
            }

        }

        public static async Task RecieveAsync(ClientWebSocket client)
        {
            var buffer = new byte[1024 * 4];
            while (true)
            {
                var result = client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                Console.WriteLine(Encoding.UTF8.GetString(buffer, 0, result.Result.Count));
                if (result.Result.MessageType == WebSocketMessageType.Close)
                {
                    await client.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                    break;
                }
            }
        }
    }
}
