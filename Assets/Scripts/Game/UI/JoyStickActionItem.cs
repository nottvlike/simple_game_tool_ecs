using Data;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class JoyStickActionItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [System.Serializable]
    public struct JoyStickActionInfo
    {
        public JoyStickActionType actionType;
        public JoyStickActionFaceType faceType;
        public KeyStateType keyStateType;
    }

    public JoyStickActionInfo[] ActionInfo;

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateActionInfo(KeyStateType.Down);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateActionInfo(KeyStateType.Up);
    }

    void UpdateActionInfo(KeyStateType keyStateType)
    {
        var player = WorldManager.Instance.Player;
        var joyStickData = player.GetData<Data.ClientJoyStickData>();
        var actorData = player.GetData<Data.ActorData>();
        for (var i = 0; i < ActionInfo.Length; i++)
        {
            var actionInfo = ActionInfo[i];
            if (actionInfo.keyStateType == keyStateType)
            {
                var defaultSkill = actionInfo.actionType == JoyStickActionType.SkillDefault ? actorData.defaultSkill : SkillDefaultType.None;
                Module.ActorJoyStick.AddJoyStickActionData(player, joyStickData, actionInfo.actionType, actionInfo.faceType, defaultSkill);
            }
        }
    }
}
