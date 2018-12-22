using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorMove : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(SpeedData));
            _requiredDataTypeList.Add(typeof(ResourceStateData));
        }

        public override void Refresh(ObjectData objData)
        {
            var resourceStateData = objData.GetData<ResourceStateData>();
            if (!resourceStateData.isInstantiated)
            {
                return;
            }

            var speedData = objData.GetData<SpeedData>();
            if (speedData.speed == 0)
            {
                return;
            }

            speedData.speed = speedData.speed - speedData.friction;
            if (speedData.speed < 0)
            {
                speedData.speed = 0;
            }

            var directionData = objData.GetData<DirectionData>();
            var deltaX = (float)directionData.x * speedData.speed / Constant.SPEED_BASE;
            var deltaY = (float)directionData.y * speedData.speed / Constant.SPEED_BASE;
            var deltaZ = (float)directionData.z * speedData.speed / Constant.SPEED_BASE;

            var resourceData = objData.GetData<ResourceData>();
            var transform = resourceData.gameObject.transform;
            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();
            var deltaTime = (float)gameSystemData.unscaleDeltaTime / Constant.SECOND_TO_MILLISECOND;
            transform.Translate(deltaX * deltaTime, deltaY * deltaTime, deltaZ * deltaTime);
        }
    }
}