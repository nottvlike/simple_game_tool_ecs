using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorLoader : Module
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(Physics2DData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
            _requiredDataTypeList.Add(typeof(ActorController2DData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            var resourceData = objData.GetData<ResourceData>();
            if (!string.IsNullOrEmpty(resourceData.resource) && resourceStateData.isGameObject)
            {
                resourceStateData.isLoaded = WorldManager.Instance.ResourceMgr.IsResourceLoaded(resourceData.resource);
                if (!resourceStateData.isInstantiated)
                {
                    LoadResource(objData, resourceStateData, resourceData);
                }
            }
        }

        void LoadResource(ObjectData objData, ResourceStateData resourceStateData, ResourceData resourceData)
        {
            WorldManager.Instance.ResourceMgr.LoadAsync(resourceData.resource, delegate (Object obj)
            {
                resourceStateData.isInstantiated = true;
                resourceData.gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
                resourceData.gameObject.name = resourceStateData.name;

                var controller = objData.GetData<ActorController2DData>();
                var rigidbody2D = resourceData.gameObject.GetComponent<Rigidbody2D>();
                controller.rigidbody2D = rigidbody2D;
                controller.foot = resourceData.gameObject.transform.Find("Foot");
                controller.positionY = Mathf.RoundToInt(controller.foot.position.y);
            });
        }
    }
}
