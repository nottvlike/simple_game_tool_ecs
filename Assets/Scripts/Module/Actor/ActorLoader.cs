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
            _requiredDataTypeList.Add(typeof(PositionData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            var resourceData = objData.GetData<ResourceData>();
            var positionData = objData.GetData<PositionData>();
            if (!string.IsNullOrEmpty(resourceData.resource) && resourceStateData.isGameObject)
            {
                resourceStateData.isLoaded = WorldManager.Instance.ResourceMgr.IsResourceLoaded(resourceData.resource);
                if (!resourceStateData.isInstantiated)
                {
                    LoadResource(resourceStateData, resourceData, positionData);
                }
            }
        }

        void LoadResource(ResourceStateData resourceStateData, ResourceData resourceData, PositionData positionData)
        {
            WorldManager.Instance.ResourceMgr.LoadAsync(resourceData.resource, delegate (Object obj)
            {
                resourceStateData.isInstantiated = true;
                resourceData.gameObject = Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
                resourceData.gameObject.name = resourceStateData.name;

                var position = resourceData.gameObject.transform.position;
                positionData.position.x = Mathf.CeilToInt(position.x * Constant.UNITY_UNIT_TO_GAME_UNIT);
                positionData.position.y = Mathf.CeilToInt(position.y * Constant.UNITY_UNIT_TO_GAME_UNIT);
                positionData.position.z = Mathf.CeilToInt(position.z * Constant.UNITY_UNIT_TO_GAME_UNIT);
            });
        }
    }
}
