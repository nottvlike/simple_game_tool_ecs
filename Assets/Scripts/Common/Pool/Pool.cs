using System;
using System.Collections.Generic;

public class Pool : IPool
{
	Dictionary<string, List<IPoolObject>> _poolObjectDict = new Dictionary<string, List<IPoolObject>>();

    public T Get<T>() where T : IPoolObject, new()
	{
        var name = typeof(T).ToString();
        IPoolObject poolObject = null;
        List<IPoolObject> poolObjectList = null;
		if (!_poolObjectDict.TryGetValue(name, out poolObjectList))
		{
            poolObjectList = new List<IPoolObject>();
			_poolObjectDict.Add(name, poolObjectList);
        }
        else
        {
            for (var i = 0; i < poolObjectList.Count; i++)
            {
                var tmpPoolObject = poolObjectList[i];
                if (!tmpPoolObject.IsInUse)
                {
                    poolObject = tmpPoolObject;
                    break;
                }
            }
        }

        if (poolObject == null)
        {
            poolObject = new T();
            poolObjectList.Add(poolObject);
        }

        poolObject.IsInUse = true;
        return (T)poolObject;
	}
	
	public void Release(IPoolObject poolObject)
	{
        List<IPoolObject> poolObjectList = null;
		if (_poolObjectDict.TryGetValue(poolObject.PoolName, out poolObjectList))
		{   
            if (poolObjectList.IndexOf(poolObject) != -1)
            {
                poolObject.IsInUse = false;
            }
            else
            {
                LogUtil.W("PoolManager Can't find PoolObject {0}!", poolObject.ToString());
            }
		}
		else
		{
			LogUtil.W("PoolManager Can't find PoolName {0}!", poolObject.PoolName);
		}
	}
    public void Destroy()
    {
    }
}