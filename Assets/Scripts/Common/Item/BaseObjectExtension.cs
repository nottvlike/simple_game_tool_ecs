using System;

public static class BaseObjectExtension
{
    public static void AddModule<T>(this BaseObject obj) where T : Module, new()
    {
        var worldMgr = WorldManager.Instance;
        var module = worldMgr.GetModule<T>();
        if (module == null)
        {
            module = new T();
        }

        var objectData = worldMgr.GetObjectData(obj.Id);
        objectData.UpdateModuleAddedObjectDict(ModuleChangeType.Add, typeof(T));

        module.OnAdd(obj);
    }

    public static void RemoveModule<T>(this BaseObject obj)
    {
        var worldMgr = WorldManager.Instance;
        worldMgr.GetObjectData(obj.Id).UpdateModuleAddedObjectDict(ModuleChangeType.Remove, typeof(T));

        var module = worldMgr.GetModule<T>();
        if (module != null)
        {
            var moduleAddedObjectList = ObjectData.GetModuleAddedObjectList<T>();
            if (moduleAddedObjectList != null && moduleAddedObjectList.Count == 0)
            {
                worldMgr.ModuleList.Remove(module);
            }
        }
        else
        {
            LogUtil.W("Remove component {0} failed, could not find module!", typeof(T).ToString());
        }
    }

    public static IModule GetModule<T>(this BaseObject obj)
    {
        var module = WorldManager.Instance.GetModule<T>();
        if (module != null)
        {
            module.CurrentObject = obj;
        }

        return module;
    }

    public static void AddData(this BaseObject obj, ValueType data)
    {
        var objectData = WorldManager.Instance.GetObjectData(obj.Id);
        objectData.DataList.Add(data);
    }

    public static void RemoveData(this BaseObject obj, ValueType data)
    {
        var objectData = WorldManager.Instance.GetObjectData(obj.Id);
        objectData.DataList.Remove(data);
    }

    public static ValueType GetData<T>(this BaseObject obj)
    {
        var dataType = typeof(T);
        var objectData = WorldManager.Instance.GetObjectData(obj.Id);

        var dataList = objectData.DataList;
        for (var i = 0; i < dataList.Count; i++)
        {
            var data = dataList[i];
            if (data.GetType() == dataType)
            {
                return data;
            }
        }

        return 0;
    }
}
