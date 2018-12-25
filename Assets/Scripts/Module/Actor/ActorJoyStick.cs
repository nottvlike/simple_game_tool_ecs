using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class ActorJoyStick : UpdateModule
    {
        protected override void InitRequiredDataType()
        {
            _requiredDataTypeList.Add(typeof(ClientJoyStickData));
        }

        public override bool IsUpdateRequired(Data.Data data)
        {
            return false;
        }

        public override void Refresh(ObjectData objData)
        {
#if !UNITY_EDITOR
            Stop(objData.ObjectId);
            return;
#endif
            var joyStickData = objData.GetData<ClientJoyStickData>();

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
                            AddJoyStickActionData(objData, joyStickData, joystickMapData.joyStickActionType, joystickMapData.joyStickActionFaceType);
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
                            AddJoyStickActionData(objData, joyStickData, joystickMapData.joyStickActionType, joystickMapData.joyStickActionFaceType);
                        }
                    }
                }
            }
        }

        public static void AddJoyStickActionData(ObjectData objData, ClientJoyStickData joyStickData, JoyStickActionType actionType, JoyStickActionFaceType faceType)
        {
            var gameSystemData = WorldManager.Instance.GameCore.GetData<GameSystemData>();

            var joyStickActionData = WorldManager.Instance.PoolMgr.Get<JoyStickActionData>();
            joyStickActionData.frame = gameSystemData.clientFrame + Constant.JOYSTICK_DELAY_FRAME_COUNT;
            joyStickActionData.actionType = actionType;
            joyStickActionData.actionParam = faceType;
            joyStickData.actionList.Add(joyStickActionData);

            objData.SetDirty(joyStickData);
        }
    }
}
