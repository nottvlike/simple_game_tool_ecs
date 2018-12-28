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
            _requiredDataTypeList.Add(typeof(Collider2DData));
            _requiredDataTypeList.Add(typeof(PositionData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
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

                var transform = resourceData.gameObject.transform;
                var collider2DData = objData.GetData<Collider2DData>();
                collider2DData.forward = transform.Find("forward");
                collider2DData.ground = transform.Find("ground");
                collider2DData.back = transform.Find("back");

                var positionData = objData.GetData<PositionData>();
                var position = transform.position;
                positionData.position.x = Mathf.CeilToInt(position.x * Constant.UNITY_UNIT_TO_GAME_UNIT);
                positionData.position.y = Mathf.CeilToInt(position.y * Constant.UNITY_UNIT_TO_GAME_UNIT);
                positionData.position.z = Mathf.CeilToInt(position.z * Constant.UNITY_UNIT_TO_GAME_UNIT);

                var physics2DData = objData.GetData<Physics2DData>();
                physics2DData.halfWidth = Mathf.CeilToInt((collider2DData.forward.position.x - position.x) * Constant.UNITY_UNIT_TO_GAME_UNIT);
                physics2DData.halfHeight = Mathf.CeilToInt((position.y - collider2DData.ground.position.y) * Constant.UNITY_UNIT_TO_GAME_UNIT);
            });
        }
    }
}
