using System.Collections.Generic;
using UnityEngine;
using Data;

namespace Module
{
    public class JoyStick : Module, IUpdateEvent
    {
        public JoyStick()
        {
            WorldManager.Instance.GetUnityEventTool().Add(this);
        }

        public override bool IsBelong(List<Data.Data> dataList)
        {
            for (var i = 0; i < dataList.Count; ++i)
            {
                var dataType = dataList[i].GetType();
                if (dataType == typeof(JoyStickData))
                {
                    return true;
                }
            }

            return false;
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
        }

        void AddJoyStickActionData(JoyStickData joyStickData, JoyStickActionType actionType, JoyStickActionFaceType faceType)
        {
            var joyStickActionData = new JoyStickActionData();
            joyStickActionData.frame = 0;
            joyStickActionData.actionType = actionType;
            joyStickActionData.actionParam = faceType;

            joyStickData.clientActionList.Add(joyStickActionData);
        }
    }
}