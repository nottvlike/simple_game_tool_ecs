using System;
using System.Collections.Generic;

public enum ModuleChangeType
{
    Add,
    Remove,
    Clear
}

public class ObjectData
{
    static Dictionary<Type, List<int>> moduleAddedObjectIdList = new Dictionary<Type, List<int>>();

    int _objId = 0;
    List<Type> _moduleTypeList = new List<Type>();
    List<Data.Data> _dataList = new List<Data.Data>();

    public int ObjectId
    {
        get { return _objId; }
    }

    public List<Type> ModuleTypeList
    {
        get { return _moduleTypeList; }
    }

    public List<Data.Data> DataList
    {
        get { return _dataList; }
    }

    public ObjectData(int objId)
    {
        _objId = objId;
    }

    public void RefreshModuleAddedObjectIdList()
    {
        var moduleList = WorldManager.Instance.ModuleList;
        for (var i = 0; i < moduleList.Count; i++)
        {
            var module = moduleList[i];
            List<int> objectIdList;
            if (!moduleAddedObjectIdList.TryGetValue(module.GetType(), out objectIdList))
            {
                objectIdList = new List<int>();
                moduleAddedObjectIdList.Add(module.GetType(), objectIdList);
            }

            var isBelong = module.IsBelong(_dataList);
            var isContains = objectIdList.Contains(_objId);
            if (!isContains && isBelong)
            {
                objectIdList.Add(_objId);
                module.OnIdListChanged();
            }
            else if (isContains && !isBelong)
            {
                objectIdList.Remove(_objId);
                module.OnIdListChanged();
            }
        }
    }

    public void AddData(Data.Data data)
    {
        _dataList.Add(data);
    }

    public void RemoveData(Data.Data data)
    {
        _dataList.Remove(data);
    }

    public Data.Data GetData<T>()  where T : Data.Data
    {
        for (var i = 0; i < _dataList.Count; i++)
        {
            var data = _dataList[i];
            if (data.GetType() == typeof(T))
            {
                return data;
            }
        }

        return null;
    }

    public static List<int> GetModuleAddedObjectList<T>()
    {
        return GetModuleAddedObjectList(typeof(T));
    }

    public static List<int> GetModuleAddedObjectList(Type moduleType)
    {
        List<int> objectList;
        moduleAddedObjectIdList.TryGetValue(moduleType, out objectList);
        return objectList;
    }
}
