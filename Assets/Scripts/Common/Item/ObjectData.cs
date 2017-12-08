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
    List<ValueType> _dataList = new List<ValueType>();

    public int ObjectId
    {
        get { return _objId; }
    }

    public List<Type> ModuleTypeList
    {
        get { return _moduleTypeList; }
    }

    public List<ValueType> DataList
    {
        get { return _dataList; }
    }

    public ObjectData(int objId)
    {
        _objId = objId;
    }

    public void UpdateModuleAddedObjectDict(ModuleChangeType dealType, Type moduleType)
    {
        switch (dealType)
        {
            case ModuleChangeType.Add:
                {
                    if (_moduleTypeList.IndexOf(moduleType) != -1)
                    {
                        LogUtil.W("Module {0} has been added on Object {1}!", moduleType.ToString(), _objId.ToString());
                    }
                    else
                    {
                        _moduleTypeList.Add(moduleType);

                        List<int> objectIdList;
                        if (!moduleAddedObjectIdList.TryGetValue(moduleType, out objectIdList))
                        {
                            objectIdList = new List<int>();
                            moduleAddedObjectIdList.Add(moduleType, objectIdList);
                        }

                        if (!objectIdList.Contains(_objId))
                        {
                            objectIdList.Add(_objId);
                        }
                        else
                        {
                            LogUtil.W("Module {0} has been added on Object {1}!", moduleType.ToString(), _objId.ToString());
                        }
                    }
                }
                break;
            case ModuleChangeType.Remove:
                {
                    if (_moduleTypeList.IndexOf(moduleType) == -1)
                    {
                        LogUtil.W("Could not find module {0} on Object {1}!", moduleType.ToString(), _objId.ToString());
                    }
                    else
                    {
                        _moduleTypeList.Remove(moduleType);

                        List<int> objectIdList;
                        moduleAddedObjectIdList.TryGetValue(moduleType, out objectIdList);
                        if (objectIdList != null && objectIdList.Contains(_objId))
                        {
                            objectIdList.Remove(_objId);
                        }
                        else
                        {
                            LogUtil.W("Could not find module {0} on Object {1}!", moduleType.ToString(), _objId.ToString());
                        }
                    }
                }
                break;
            case ModuleChangeType.Clear:
                {
                    List<int> objectIdList;
                    for (var i = 0; i < _moduleTypeList.Count; i++)
                    {
                        if (moduleAddedObjectIdList.TryGetValue(moduleType, out objectIdList)
                            && objectIdList.Contains(_objId))
                        {
                            objectIdList.Remove(_objId);
                        }
                        else
                        {
                            LogUtil.W("Could not find module {0} on Object {1}!", moduleType.ToString(), _objId.ToString());
                        }
                    }

                    _moduleTypeList.Clear();
                }
                break;
        }
    }

    public static List<int> GetModuleAddedObjectList<T>()
    {
        var moduleType = typeof(T);
        List<int> objectList;
        moduleAddedObjectIdList.TryGetValue(moduleType, out objectList);
        return objectList;
    }
}
