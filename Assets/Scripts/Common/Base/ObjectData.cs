using System;
using System.Collections.Generic;

public class ObjectData : IPoolObject
{
    static int _idGenerate = 0;

    int _objId = 0;
    List<Data.Data> _dataList = new List<Data.Data>();

    public int ObjectId
    {
        get { return _objId; }
    }

    public List<Data.Data> DataList
    {
        get { return _dataList; }
    }

    public ObjectData()
    {
        _objId = ++_idGenerate;
    }

    public void SetDirty(params Data.Data[] dataList)
    {
        var moduleList = WorldManager.Instance.ModuleList;
        for (var i = 0; i < moduleList.Count; i++)
        {
            var module = moduleList[i];

            var isMeet = module.IsMeet(_dataList);
            var isContains = module.Contains(_objId);
            if (!isContains && isMeet)
            {
                module.Add(_objId);
            }
            else if (isContains && !isMeet)
            {
                module.Remove(_objId);
            }
            else if (isMeet && isContains)
            {
                var needUpdate = false;
                for (var j = 0; j < dataList.Length; j++)
                {
                    if (module.IsUpdateRequired(dataList[j]))
                    {
                        needUpdate = true;
                        break;
                    }
                }

                if (needUpdate)
                {
                    module.SetDirty(this);
                }
            }
        }
    }

    public void AddData(Data.Data data)
    {
#if UNITY_EDITOR
        var tmpData = GetData(data.GetType());
        if (tmpData != null)
        {
            LogUtil.W("Type {0} has been added!", data.GetType());
            return;
        }
#endif

        _dataList.Add(data);
    }

    public void RemoveData(Data.Data data)
    {
#if UNITY_EDITOR
        var tmpData = GetData(data.GetType());
        if (tmpData == null)
        {
            LogUtil.W("Type {0} could not been found!", data.GetType());
            return;
        }
#endif

        _dataList.Remove(data);
        WorldManager.Instance.PoolMgr.ReleaseData(data);
    }

    public Data.Data GetData(Type type)
    {
        for (var i = 0; i < _dataList.Count; i++)
        {
            var data = _dataList[i];
            if (data.GetType() == type)
            {
                return data;
            }
        }

        return null;
    }

    public T AddData<T>() where T : Data.Data, new()
    {
#if UNITY_EDITOR
        T data = GetData<T>();
        if (data != null)
        {
            LogUtil.W("Type {0} has been added!", typeof(T));
            return data;
        }
#endif

        data = WorldManager.Instance.PoolMgr.GetData(typeof(T)) as T;
        _dataList.Add(data);

        return data;
    }

    public void RemoveData<T>() where T : Data.Data
    {
#if UNITY_EDITOR
        if (GetData<T>() == null)
        {
            LogUtil.W("Type {0} could not been found!", typeof(T));
            return;
        }
#endif

        for (var i = 0; i < _dataList.Count; i++)
        {
            var data = _dataList[i];
            if (data.GetType() == typeof(T))
            {
                _dataList.Remove(data);
                WorldManager.Instance.PoolMgr.ReleaseData(data);
            }
        }
    }

    public T GetData<T>()  where T : Data.Data
    {
        return GetData(typeof(T)) as T;
    }

    public bool IsInUse
    {
        get;
        set;
    }

    public void Clear()
    {
    }
}
