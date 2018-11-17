using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    IPool _poolMgr;
    public IPool PoolMgr
    {
        get
        {
            if (_poolMgr == null)
                _poolMgr = new Pool();

            return _poolMgr;
        }
    }

    void DestroyPoolMgr()
    {
        if (_poolMgr != null)
        {
            _poolMgr.Destroy();
            _poolMgr = null;
        }
    }
}
