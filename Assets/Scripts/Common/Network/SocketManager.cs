using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    ISocketTool _socketMgr;
    public ISocketTool SocketMgr
    {
        get
        {
            if (_socketMgr == null)
                _socketMgr = new TcpSocket();

            return _socketMgr;
        }
    }

    void DestroySocketMgr()
    {
        if (_socketMgr != null)
        {
            _socketMgr.Destroy();
            _socketMgr = null;
        }
    }
}
