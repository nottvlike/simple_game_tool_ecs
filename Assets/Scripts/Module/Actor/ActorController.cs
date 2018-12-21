using System.Collections;
using System.Collections.Generic;
using Data;
using UnityEngine;

namespace Module
{
    public class ActorController : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(PositionData));
            _requiredDataTypeList.Add(typeof(DirectionData));
            _requiredDataTypeList.Add(typeof(SpeedData));
            _requiredDataTypeList.Add(typeof(JoyStickData));
        }

        public override void Refresh(ObjectData objData, bool notMet = false)
        {
            if (notMet)
            {
                return;
            }

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
                    switch (serverAction.actionType)
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
