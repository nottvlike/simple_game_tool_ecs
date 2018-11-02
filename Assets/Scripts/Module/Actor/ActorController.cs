using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorController : UpdateModule
    {
        public ActorController()
            : base()
        {
        }

        public override bool IsBelong(List<Data.Data> dataList)
        {
            var index = 0;
            for (var i = 0; i < dataList.Count; ++i)
            {
                var dataType = dataList[i].GetType();
                if (dataType == typeof(PositionData) || dataType == typeof(DirectionData) || dataType == typeof(SpeedData) || dataType == typeof(JoyStickData))
                {
                    index++;
                }
            }
            return index == 4;
        }

        protected override void UpdateObject(int objId, ObjectData objData)
        {
            var joyStickData = objData.GetData<JoyStickData>() as JoyStickData;

            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>() as GameSystemData;

            var speedData = objData.GetData<SpeedData>() as SpeedData;
            var directionData = objData.GetData<DirectionData>() as DirectionData;

            var serverActionList = joyStickData.serverActionList;
            for (var i = 0; i < serverActionList.Count; i++)
            {
                var serverAction = serverActionList[i];
                if (serverAction.frame == gameSystemData.clientFrame)
                {
                    switch(serverAction.actionType)
                    {
                        case JoyStickActionType.Run:
                            speedData.acceleration = 100;
                            speedData.accelerationDelta = 0;
                            directionData.x = serverAction.actionParam == JoyStickActionFaceType.Right ? 1 : -1;
                            break;
                        case JoyStickActionType.CancelRun:
                            speedData.accelerationDelta = 10;
                            break;
                    }
                }
            }
        }
    }
}
