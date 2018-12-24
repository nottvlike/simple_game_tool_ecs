using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorFollowCamera : UpdateModule
    {
        TimerEvent _timer;
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(FollowCameraData));
            _requiredDataTypeList.Add(typeof(ResourceData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var resourceData = objData.GetData<ResourceData>();
            var followCameraData = objData.GetData<FollowCameraData>();
            followCameraData.interval += gameSystemData.unscaleDeltaTime;
            if (followCameraData.interval >= 1000)
            {
                followCameraData.interval = 0;
                followCameraData.targetPosition = resourceData.gameObject.transform.position;
            }

            var cameraPosition = Camera.main.transform.position;
            var targetPosition = followCameraData.targetPosition;
            if (cameraPosition.x != targetPosition.x 
                || cameraPosition.y != targetPosition.y)
            {
                var directionX = targetPosition.x > cameraPosition.x ? 1 : -1;
                var directionY = targetPosition.y > cameraPosition.y ? 1 : -1;
                var deltaTime = gameSystemData.unscaleDeltaTime / (float)Constant.SECOND_TO_MILLISECOND;
                var deltaX = directionX * followCameraData.moveSpeed / Constant.SPEED_BASE * deltaTime;
                var deltaY = directionY * followCameraData.moveSpeed / Constant.SPEED_BASE * deltaTime;

                if (Mathf.Abs(deltaX) > Mathf.Abs(targetPosition.x - cameraPosition.x))
                {
                    deltaX = targetPosition.x - cameraPosition.x;
                }

                if (Mathf.Abs(deltaY) > Mathf.Abs(targetPosition.y - cameraPosition.y))
                {
                    deltaY = targetPosition.y - cameraPosition.y;
                }
                Camera.main.transform.Translate(deltaX, deltaY, 0);
            }
        }

        public void Tick()
        {
            for (var i = 0; i < _objectIdList.Count; ++i)
            {
                var objId = _objectIdList[i];

                var objData = WorldManager.Instance.GetObjectData(objId);
                var resourceStateData = objData.GetData<ResourceStateData>();
                if (!resourceStateData.isInstantiated)
                {
                    return;
                }

                var resourceData = objData.GetData<ResourceData>();
                var transform = resourceData.gameObject.transform;

                var targetPosition = transform.position;
                var cameraPosition = Camera.main.transform.position;
                var deltaX = (targetPosition.x - cameraPosition.x) * 0.4f;
                var deltaY = (targetPosition.y - cameraPosition.y) * 0.4f;
                Camera.main.transform.Translate(deltaX, deltaY, 0);
            }


        }
    }
}
