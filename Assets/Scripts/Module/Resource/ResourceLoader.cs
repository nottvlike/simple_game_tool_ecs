using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ResourceLoader : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>() as ResourceStateData;
            var resourceData = objData.GetData<ResourceData>() as ResourceData;
            if (!string.IsNullOrEmpty(resourceData.resource) && resourceStateData.isGameObject)
            {
                resourceStateData.isLoaded = WorldManager.Instance.ResourceMgr.IsResourceLoaded(resourceData.resource);
                if (!resourceStateData.isInstantiated)
                {
                    LoadResource(resourceStateData, resourceData);
                }
            }
        }

        void LoadResource(ResourceStateData resourceStateData, ResourceData resourceData)
        {
            WorldManager.Instance.ResourceMgr.LoadAsync(resourceData.resource, delegate (Object obj)
            {
                resourceStateData.isInstantiated = true;
                resourceData.gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
                resourceData.gameObject.name = resourceStateData.name;
            });
        }
    }
}
