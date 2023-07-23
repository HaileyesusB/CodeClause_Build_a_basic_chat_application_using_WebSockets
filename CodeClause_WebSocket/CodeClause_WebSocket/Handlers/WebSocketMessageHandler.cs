using CodeClause_WebSocket.SocketManager;
using System.Net.WebSockets;
using System.Text;

namespace CodeClause_WebSocket.Handlers
{
    public class WebSocketMessageHandler : SocketHandler
    {
        private readonly ConnectionManager _connections;
        public WebSocketMessageHandler(ConnectionManager connections): base(connections)
        {
          _connection = connections;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);
            var socketId = _connections.GetId(socket);
            await SendMessageToAll($"{socketId}  just joined the party ***");
        }
        public override async Task Recieve(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
           var socketId = _connection.GetId(socket);
            var message = $"{socketId} said: {Encoding.UTF8.GetString(buffer, 0 , result.Count)}";
            await SendMessageToAll(message);
        }
    }
}
