using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ConnectionState
{
    None,
    NotConnect,
    Connected
}

public delegate void ReceiveMessageDelegate(Message msg);

public struct Message
{
    public byte[] data;
    public uint reqId;
    public uint resId;
    public ReceiveMessageDelegate callback;
}

public interface ISocketTool 
{
    void Init(string ip, int port);
    void Destroy();

    void SendMessage(byte[] buf, uint reqId, uint resId = 0, ReceiveMessageDelegate callback = null);
}
