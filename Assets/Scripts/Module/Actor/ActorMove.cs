using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorMove : UpdateModule
    {
        public ActorMove()
            : base()
        {
        }

        public override bool IsBelong(List<Data.Data> dataList)
        {
            var index = 0;
            for (var i = 0; i < dataList.Count; ++i)
            {
                var dataType = dataList[i].GetType();
                if (dataType == typeof(PositionData) || dataType == typeof(DirectionData) || dataType == typeof(SpeedData))
                {
                    index++;
                }
            }
            return index == 3;
        }

        protected override void UpdateObject(int objId, BaseObject obj)
        {
            var objData = WorldManager.Instance.GetObjectData(objId);
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

            var transform = ((GameObject)obj.Resource).transform;
            var deltaSpeed = Time.deltaTime / Constant.SPEED;
            transform.Translate(deltaX * Time.deltaTime / Constant.SPEED, deltaY * Time.deltaTime / Constant.SPEED, deltaZ * Time.deltaTime / Constant.SPEED);
        }
    }
}