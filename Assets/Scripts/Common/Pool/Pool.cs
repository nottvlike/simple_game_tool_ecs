using System;
using System.Collections.Generic;
using Data;

public class Pool : IPool
{
    Dictionary<Type, List<IPoolObject>> _poolObjectDict = new Dictionary<Type, List<IPoolObject>>();

    public Data.Data GetData(Type type)
    {
        return Get(typeof(Data.Data), type) as Data.Data;
    }

    public void ReleaseData(Data.Data data)
    {
        Release(typeof(Data.Data), data);
    }

    public ObjectData GetObjData()
    {
        return Get(typeof(ObjectData), typeof(ObjectData)) as ObjectData;
    }

    public ObjectData GetObjData(ObjectData objData)
    {
        var poolObjData = GetObjData();

        var dataList = objData.DataList;
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            poolObjData.AddData(GetData(data.GetType()));
        }

        WorldManager.Instance.ObjectDataList.Add(poolObjData);

        return objData;
    }

    public void ReleaseObjData(ObjectData objData)
    {
        var dataList = objData.DataList;
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            ReleaseData(data);
        }

        dataList.Clear();

        objData.SetDirty();

        WorldManager.Instance.ObjectDataList.Remove(objData);

        Release(typeof(ObjectData), objData);
    }

    public T Get<T>() where T : IPoolObject
    {
        return (T)Get(typeof(T), typeof(T));
    }

    public void Release(IPoolObject poolObject)
    {
        Release(poolObject.GetType(), poolObject);
    }

    IPoolObject Get(Type poolType, Type objType)
    {
        IPoolObject poolObject = null;
        List<IPoolObject> poolObjectList = null;
        if (!_poolObjectDict.TryGetValue(poolType, out poolObjectList))
        {
            poolObjectList = new List<IPoolObject>();
            _poolObjectDict.Add(poolType, poolObjectList);
        }
        else
        {
            for (var i = 0; i < poolObjectList.Count; i++)
            {
                var tmp = poolObjectList[i];
                if (!tmp.IsInUse)
                {
                    tmp.IsInUse = true;
                    poolObject = tmp;
                    break;
                }
            }
        }

        if (poolObject == null)
        {
            poolObject = Activator.CreateInstance(objType) as IPoolObject;
            poolObject.IsInUse = true;

            poolObjectList.Add(poolObject);
        }

        return poolObject;
    }

    void Release(Type poolType, IPoolObject poolObject)
    {
        List<IPoolObject> poolObjectList = null;
        if (_poolObjectDict.TryGetValue(poolType, out poolObjectList))
        {
            if (poolObjectList.IndexOf(poolObject) != -1)
            {
                poolObject.Clear();
                poolObject.IsInUse = false;
            }
            else
            {
                LogUtil.W("PoolManager Can't find PoolObject {0}!", poolObject.ToString());
            }
        }
        else
        {
            LogUtil.W("PoolManager Can't find PoolName {0}!", poolType.ToString());
        }
    }
    
    public void Destroy()
    {
    }
}