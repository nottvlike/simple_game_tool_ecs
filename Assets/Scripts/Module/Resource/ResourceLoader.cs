using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Module
{
    public class ResourceLoader : Module
    {
        public override bool IsBelong(List<Data.Data> dataList)
        {
            var index = 0;
            for (var i = 0; i < dataList.Count; ++i)
            {
                var dataType = dataList[i].GetType();
                if (dataType == typeof(Data.ResourceData) || dataType == typeof(Data.ResourceStateData))
                {
                    index++;
                }
            }

            return index == 2;
        }

        public override void OnAdd(int objId)
        {
            base.OnAdd(objId);

            var worldMgr = WorldManager.Instance;
            var resourceMgr = worldMgr.ResourceMgr;
            var objData = worldMgr.GetObjectData(objId);
            var resourceStateData = objData.GetData<Data.ResourceStateData>() as Data.ResourceStateData;
            var resourceData = objData.GetData<Data.ResourceData>() as Data.ResourceData;
            resourceStateData.isLoaded = resourceMgr.IsResourceLoaded(resourceData.resource);
            if (!resourceStateData.isGameObject || (resourceStateData.isLoaded && resourceStateData.isInstantiated) )
            {
                return;
            }

            resourceMgr.LoadAsync(resourceData.resource, delegate (Object obj)
            {
                resourceStateData.isInstantiated = true;
                resourceData.gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
                resourceData.gameObject.name = resourceData.name;
            });
        }

        public override void OnRemove(int objId)
        {
            base.OnRemove(objId);

            var worldMgr = WorldManager.Instance;
            var objData = worldMgr.GetObjectData(objId);
            var resourceStateData = objData.GetData<Data.ResourceStateData>() as Data.ResourceStateData;
            var resourceData = objData.GetData<Data.ResourceData>() as Data.ResourceData;

            Object.Destroy(resourceData.gameObject);
            resourceStateData.isInstantiated = false;
        }
    }
}
