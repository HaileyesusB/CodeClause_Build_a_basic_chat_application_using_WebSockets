using System.Collections.Concurrent;
using System.Net.WebSockets;

namespace CodeClause_WebSocket.SocketManager
{
    public class ConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _connection = new ConcurrentDictionary<string, WebSocket>();
        public WebSocket GetSocketById(string id)
        {
            return _connection.FirstOrDefault(x => x.Key == id).Value;
        }

        public ConcurrentDictionary<string, WebSocket> GetAllConnections() 
        {
            return _connection; 
        }

        public string GetId(WebSocket socket) 
        {
          return _connection.FirstOrDefault(x=>x.Value == socket).Key;
        }

        public async Task RemoveSocketAsync(string id)
        {
            _connection.TryRemove(id, out WebSocket socket);
            await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Socket Connection Closed", CancellationToken.None);
        }

        private string GetConnectionId()
        {
            return Guid.NewGuid().ToString("N");
        }

        public void AddSocket(WebSocket socket)
        {
            _connection.TryAdd(GetConnectionId(), socket);
        }

    }
}
