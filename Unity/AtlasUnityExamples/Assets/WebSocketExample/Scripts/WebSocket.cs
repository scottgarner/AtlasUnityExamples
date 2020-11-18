using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public class WebSocketReceiveEvent : UnityEvent<string> { }

[System.Serializable]
public class WebSocketReceiveBinaryEvent : UnityEvent<byte[]> { }

public class WebSocket : MonoBehaviour
{
    public string webSocketUrl = "wss://atlas-workshop-server.glitch.me";

    public WebSocketBridge webSocketBridge;

    public WebSocketReceiveEvent OnReceive;
    public WebSocketReceiveBinaryEvent OnReceiveBinary;

    void OnEnable()
    {
        webSocketBridge = new WebSocketBridge(webSocketUrl);

        webSocketBridge.OnReceive += Receive;
        webSocketBridge.OnReceiveBinary += ReceiveBinary;
    }

    void OnDisable()
    {
        webSocketBridge.OnReceive -= Receive;
        webSocketBridge.OnReceiveBinary -= ReceiveBinary;

        webSocketBridge.Close();
    }

    public void Send(string message)
    {
        webSocketBridge.Send(message);
    }

    public void Receive(string message)
    {
        Debug.Log(message);
        OnReceive.Invoke(message);
    }

    public void ReceiveBinary(byte[] bytes)
    {
        string hex = System.BitConverter.ToString(bytes);
        Debug.Log(hex);
        OnReceiveBinary.Invoke(bytes);
    }

}
