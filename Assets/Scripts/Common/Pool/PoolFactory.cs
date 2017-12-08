using System;
using System.Collections.Generic;

public partial class WorldManager : Singleton<WorldManager>
{
    IPool _pool;
    public IPool GetPool()
    {
        if (_pool == null)
            _pool = new Pool();

        return _pool;
    }

    public void DestroyPool()
    {
        _pool.Destroy();
        _pool = null;
    }
}
