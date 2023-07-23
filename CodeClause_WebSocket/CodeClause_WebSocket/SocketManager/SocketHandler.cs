using System.Net.WebSockets;
using System.Text;

namespace CodeClause_WebSocket.SocketManager
{
    public abstract class SocketHandler
    {
        public ConnectionManager _connection { get; set; }

        public SocketHandler(ConnectionManager connection)
        {
            _connection= connection;
        }

        public virtual async Task OnConnected(WebSocket socket)
        {
            await Task.Run(() => { _connection.AddSocket(socket); });
        }

        public virtual async Task OnDisConnected(WebSocket socket)
        {
            await _connection.RemoveSocketAsync(_connection.GetId(socket));
        }

        public async Task SendMessage(WebSocket socket, string message)
        {
            if(socket.State != WebSocketState.Open) 
            {
                return;
            }
            await socket.SendAsync(new ArraySegment<byte>(Encoding.ASCII.
                GetBytes(message), 0, message.Length), 
                WebSocketMessageType.Text, true, CancellationToken.None);
        }
        public async Task SendMessage(string id , string message)
        {
            await SendMessage(_connection.GetSocketById(id), message);
        }

        public async Task SendMessageToAll(string message)
        {
            foreach(var conn in _connection.GetAllConnections()) 
              await SendMessage(conn.Value, message);  
        }
        public abstract Task Recieve(WebSocket socket, WebSocketReceiveResult result, byte[] buffer);

    }
}
