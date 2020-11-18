using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.WebSockets;

using Debug = UnityEngine.Debug;

public class WebSocketBridge
{
    public delegate void ReceiveAction(string message);
    public event ReceiveAction OnReceive;

    public delegate void ReceiveBinaryAction(byte[] bytes);
    public event ReceiveBinaryAction OnReceiveBinary;

    private ClientWebSocket webSocket;

    private CancellationTokenSource cancellationTokenSource;
    private CancellationToken cancellationToken;

    public WebSocketBridge(string webSocketURL)
    {
        if (!String.IsNullOrEmpty(webSocketURL))
        {
            Connect(webSocketURL);
        }
    }

    public void Close()
    {
        if (cancellationTokenSource != null)
        {
            cancellationTokenSource.Cancel();
            cancellationTokenSource.Dispose();
            cancellationTokenSource = null;
        }
    }

    public async void Connect(string url)
    {
        cancellationTokenSource = new CancellationTokenSource();
        cancellationToken = cancellationTokenSource.Token;

        while (!cancellationToken.IsCancellationRequested)
        {
            using (webSocket = new ClientWebSocket())
            {

                webSocket.Options.SetRequestHeader("User-Agent", "Unity3D");

                try
                {
                    Debug.Log("<color=cyan>WebSocket connecting.</color>");
                    await webSocket.ConnectAsync(new Uri(url), cancellationToken);

                    Debug.Log("<color=cyan>WebSocket receiving.</color>");
                    await Receive();

                    Debug.Log("<color=cyan>WebSocket closed.</color>");

                }
                catch (OperationCanceledException)
                {
                    Debug.Log("<color=cyan>WebSocket shutting down.</color>");
                }
                catch (WebSocketException e)
                {
                    Debug.Log("<color=cyan>WebSocket connection lost.</color>");
                    Debug.LogWarning(e.Message);
                    Debug.LogWarning(e.WebSocketErrorCode);
                    Debug.LogWarning(e.ErrorCode);
                }
                catch (Exception ex)
                {
                    Debug.LogWarning(ex);
                    throw;
                }
            }

            Debug.Log("<color=cyan>Websocket reconnecting.</color>");
            await Task.Delay(1000);
        }
    }

    public void Send(string message)
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(message);
            ArraySegment<byte> sendBuffer = new ArraySegment<byte>(byteArray, 0, byteArray.Length);
            webSocket.SendAsync(sendBuffer, WebSocketMessageType.Text, true, cancellationToken);
        }
    }

    public void SendBinary(byte[] byteArray)
    {
        if (webSocket != null && webSocket.State == WebSocketState.Open)
        {
            ArraySegment<byte> sendBuffer = new ArraySegment<byte>(byteArray, 0, byteArray.Length);
            webSocket.SendAsync(sendBuffer, WebSocketMessageType.Binary, true, cancellationToken);
        }
    }

    private async Task Receive()
    {
        ArraySegment<byte> receiveBuffer = new ArraySegment<byte>(new byte[8192]);
        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(receiveBuffer, cancellationToken);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = Encoding.UTF8.GetString(receiveBuffer.Array, 0, result.Count);
                if (OnReceive != null) OnReceive(message);
            }
            else if (result.MessageType == WebSocketMessageType.Binary)
            {

                byte[] bytes = new byte[result.Count];
                Array.Copy(receiveBuffer.Array, bytes, result.Count);

                if (OnReceiveBinary != null) OnReceiveBinary(bytes);
            }
        }
    }
}