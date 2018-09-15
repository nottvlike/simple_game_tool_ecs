using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveModule : Module, IUpdateEvent
{
    public MoveModule()
    {
        WorldManager.Instance.GetUnityEventTool().Add(this);
    }

    public override bool IsBelong(List<Data> dataList)
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

    public void Update()
    {
        if (_objectIdList.Count == 0)
        {
            return;
        }

        for (var i = 0; i < _objectIdList.Count; ++i)
        {
            var objId = _objectIdList[i];

            var obj = WorldManager.Instance.GetObject(objId);
            if (!obj.IsInit())
            {
                continue;
            }

            var objData = WorldManager.Instance.GetObjectData(objId);
            var speedData = objData.GetData<SpeedData>() as SpeedData;
            if (speedData.acceleration == 0)
            {
                continue;
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