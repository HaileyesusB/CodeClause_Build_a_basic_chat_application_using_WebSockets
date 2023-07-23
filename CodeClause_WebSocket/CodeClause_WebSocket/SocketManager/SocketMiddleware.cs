using System.Net.WebSockets;

namespace CodeClause_WebSocket.SocketManager
{
    public class SocketMiddleware
    {
        private readonly RequestDelegate _next;
        private SocketHandler _handler { get; set; }
        public SocketMiddleware(RequestDelegate next, SocketHandler handler)
        {
            _next = next;
            _handler = handler;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if(!context.WebSockets.IsWebSocketRequest)
                return;
            var socket = await context.WebSockets.AcceptWebSocketAsync();
            await _handler.OnConnected(socket);  
            await Recieve(socket, async(result, buffer) =>
            {
              if(result.MessageType == WebSocketMessageType.Text) 
                {
                    await _handler.Recieve(socket, result, buffer);
                }
                else if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _handler.OnDisConnected(socket);
                }
            });
        }

        private async Task Recieve(WebSocket webSocket, Action<WebSocketReceiveResult, 
            byte[]> messageToHandel )
        {
            var buffer = new byte[1024 * 4];
            while(webSocket.State!= WebSocketState.Open) 
            {
                var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                MessageHandler(result, buffer);
            }
        }

        private void MessageHandler(WebSocketReceiveResult result, byte[] buffer)
        {
            throw new NotImplementedException();
        }
    }
}
