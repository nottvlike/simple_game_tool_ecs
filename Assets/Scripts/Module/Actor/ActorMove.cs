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
            _requiredDataTypeList.Add(typeof(PositionData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(SpeedData));
        }

        public override void Refresh(ObjectData objData, bool notMet = false)
        {
            if (notMet)
            {
                return;
            }

            var speedData = objData.GetData<SpeedData>() as SpeedData;
            if (speedData.acceleration == 0)
            {
                return;
            }

            var curSpeed = speedData.lastAcceleration > speedData.acceleration ? speedData.noraml : speedData.noraml + speedData.delta;

            speedData.lastAcceleration = speedData.acceleration;
            speedData.acceleration = speedData.acceleration - speedData.accelerationDelta;

            var directionData = objData.GetData<DirectionData>() as DirectionData;
            var positionData = objData.GetData<PositionData>() as PositionData;
            var deltaX = directionData.x * curSpeed;
            var deltaY = directionData.y * curSpeed;
            var deltaZ = directionData.z * curSpeed;
            positionData.x += deltaX;
            positionData.y += deltaY;
            positionData.z += deltaZ;

            var resourceData = objData.GetData<Data.ResourceData>() as Data.ResourceData;
            var transform = resourceData.gameObject.transform;
            var deltaSpeed = Time.deltaTime / Constant.SPEED;
            transform.Translate(deltaX * Time.deltaTime / Constant.SPEED, deltaY * Time.deltaTime / Constant.SPEED, deltaZ * Time.deltaTime / Constant.SPEED);
        }
    }
}