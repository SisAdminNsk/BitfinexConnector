using System.Net.WebSockets;
using System.Text;

namespace StockExchangeCore
{
    public class WebSocketClient : IDisposable
    {
        private readonly ClientWebSocket _webSocket;
        private readonly string _webSocketUrl;

        public event Action<string> OnMessageReceived;

        public WebSocketClient(string webSocketUrl)
        {
            _webSocketUrl = webSocketUrl;
            _webSocket = new ClientWebSocket();
        }

        public async Task ConnectAsync()
        {
            await _webSocket.ConnectAsync(new Uri(_webSocketUrl), CancellationToken.None);
            _ = ReceiveMessagesAsync();
        }

        public async Task SendMessageAsync(string message)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await _webSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[1024 * 8];

            while (_webSocket.State == WebSocketState.Open)
            {
                var result = await _webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
                    break;
                }

                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                OnMessageReceived?.Invoke(message);
            }
        }
        public void Dispose()
        {
            _webSocket.Dispose();
        }
    }
}
