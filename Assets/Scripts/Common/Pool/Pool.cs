using System;
using System.Collections.Generic;

public class Pool : IPool
{
	Dictionary<string, List<ObjectData>> _objectDataDict = new Dictionary<string, List<ObjectData>>();

    public ObjectData Get(ObjectData data)
    {
        if (!CheckIsPoolObject(data))
        {
            return null;
        }

        var resourceData = data.GetData<Data.ResourceData>() as Data.ResourceData;

        ObjectData poolObjectData = null;
        List<ObjectData> objectDataList = null;
        if (!_objectDataDict.TryGetValue(resourceData.name, out objectDataList))
        {
            objectDataList = new List<ObjectData>();
            _objectDataDict.Add(resourceData.name, objectDataList);
        }
        else
        {
            for (var i = 0; i < objectDataList.Count; i++)
            {
                var tmpObjectData = objectDataList[i];
                var resourcePoolData = tmpObjectData.GetData<Data.ResourcePoolData>() as Data.ResourcePoolData;
                if (!resourcePoolData.isInUse)
                {
                    resourcePoolData.isInUse = true;
                    poolObjectData = tmpObjectData;
                    break;
                }
            }
        }

        if (poolObjectData == null)
        {
            poolObjectData = DeepClone(data);
            var resourcePoolData = poolObjectData.GetData<Data.ResourcePoolData>() as Data.ResourcePoolData;
            resourcePoolData.isInUse = true;

            objectDataList.Add(poolObjectData);
        }

        return poolObjectData;
    }

    bool CheckIsPoolObject(ObjectData objData)
    {
        var poolData = objData.GetData<Data.ResourcePoolData>();
        if (poolData == null)
        {
            LogUtil.W("Not a pool object {0}!", objData.ObjectId);
            return false;
        }

        var resourceData = objData.GetData<Data.ResourceData>() as Data.ResourceData;
        if (resourceData == null)
        {
            LogUtil.W("ResourceData not exist {0}!", objData.ObjectId);
            return false;
        }

        return true;
    }

    ObjectData DeepClone(ObjectData cloneObjData)
    {
        var objData = new ObjectData();

        var dataList = cloneObjData.DataList;
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            objData.AddData(data.Clone());
        }

        var resourceStateData = objData.GetData<Data.ResourceStateData>() as Data.ResourceStateData;
        if (resourceStateData != null)
        {
            resourceStateData.isInstantiated = false;
        }

        objData.ModuleTypeList.AddRange(cloneObjData.ModuleTypeList);
        WorldManager.Instance.ObjectDataList.Add(objData);

        objData.RefreshModuleAddedObjectIdList();
        return objData;
    }
	
	public void Release(ObjectData objData)
	{
        if (!CheckIsPoolObject(objData))
        {
            return;
        }

        var poolData = objData.GetData<Data.ResourcePoolData>() as Data.ResourcePoolData;
        List<ObjectData> poolObjectList = null;
		if (_objectDataDict.TryGetValue(poolData.name, out poolObjectList))
		{   
            if (poolObjectList.IndexOf(objData) != -1)
            {
                poolData.isInUse = false;
            }
            else
            {
                LogUtil.W("PoolManager Can't find PoolObject {0}!", objData.ObjectId);
            }
		}
		else
		{
			LogUtil.W("PoolManager Can't find PoolName {0}!", poolData.name);
		}
	}

    Dictionary<Type, List<IPoolObject>> _poolObjectDict = new Dictionary<Type, List<IPoolObject>>();

    public T Get<T>() where T : IPoolObject, new()
    {
        var type = typeof(T);

        IPoolObject poolObject = null;
        List<IPoolObject> poolObjectList = null;
        if (!_poolObjectDict.TryGetValue(type, out poolObjectList))
        {
            poolObjectList = new List<IPoolObject>();
            _poolObjectDict.Add(type, poolObjectList);
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
            poolObject = new T();
            poolObject.IsInUse = true;

            poolObjectList.Add(poolObject);
        }

        return (T)poolObject;
    }

    public void Release<T>(T obj) where T : IPoolObject, new()
    {
        var type = typeof(T);

        List<IPoolObject> poolObjectList = null;
        if (_poolObjectDict.TryGetValue(type, out poolObjectList))
        {
            if (poolObjectList.IndexOf(obj) != -1)
            {
                obj.Clear();
                obj.IsInUse = false;
            }
            else
            {
                LogUtil.W("PoolManager Can't find PoolObject {0}!", obj.ToString());
            }
        }
        else
        {
            LogUtil.W("PoolManager Can't find PoolName {0}!", type.ToString());
        }
    }

    public void Destroy()
    {
    }
}