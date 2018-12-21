using System.Collections.Generic;
using UnityEngine;
using Data;

#if UNITY_EDITOR
namespace Module
{
    public class ActorJoyStick : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(JoyStickData));
        }

        public override void Refresh(ObjectData objData)
        {
            var joyStickData = objData.GetData<JoyStickData>() as JoyStickData;

            var joyStickMapDataList = WorldManager.Instance.JoyStickConfig.joyStickMapDataList;
            for (var j = 0; j < joyStickMapDataList.Length; j++)
            {
                var joystickMapData = joyStickMapDataList[j];
                if (joystickMapData.keyStateType == KeyStateType.Down)
                {
                    var keyCodeList = joystickMapData.keyCode;
                    for (var k = 0; k < keyCodeList.Length; k++)
                    {
                        if (Input.GetKeyDown(keyCodeList[k]))
                        {
                            AddJoyStickActionData(joyStickData, joystickMapData.joyStickActionType, joystickMapData.joyStickActionFaceType);
                        }
                    }
                }
                else if (joystickMapData.keyStateType == KeyStateType.Up)
                {
                    var keyCodeList = joystickMapData.keyCode;
                    for (var k = 0; k < keyCodeList.Length; k++)
                    {
                        if (Input.GetKeyUp(keyCodeList[k]))
                        {
                            AddJoyStickActionData(joyStickData, joystickMapData.joyStickActionType, joystickMapData.joyStickActionFaceType);
                        }
                    }
                }
            }
        }

        void AddJoyStickActionData(JoyStickData joyStickData, JoyStickActionType actionType, JoyStickActionFaceType faceType)
        {
            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>() as GameSystemData;

            var joyStickActionData = WorldManager.Instance.PoolMgr.Get<JoyStickActionData>();
            joyStickActionData.frame = gameSystemData.clientFrame + Constant.JOYSTICK_DELAY_FRAME_COUNT;
            joyStickActionData.actionType = actionType;
            joyStickActionData.actionParam = faceType;

            joyStickData.clientActionList.Add(joyStickActionData);
        }
    }
}
#endif