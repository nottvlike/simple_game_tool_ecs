using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    ISocketTool _socket;
    public ISocketTool GetSocketTool()
    {
        if (_socket == null)
            _socket = new TcpSocket();

        return _socket;
    }

    public void DestroySocket()
    {
        _socket.Destroy();
        _socket = null;
    }
}
