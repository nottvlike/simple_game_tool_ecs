﻿using Data;
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

    public void OnDisable()
    {
        _player = null;
    }

    ObjectData _player = null;
    void UpdateActionInfo(KeyStateType keyStateType)
    {
        if (_player == null)
        {
            _player = GetPlayer();
        }

        var joyStickData = _player.GetData<Data.ClientJoyStickData>();
        for (var i = 0; i < ActionInfo.Length; i++)
        {
            var actionInfo = ActionInfo[i];
            if (actionInfo.keyStateType == keyStateType)
            {
                Module.ActorJoyStick.AddJoyStickActionData(_player, joyStickData, actionInfo.actionType, actionInfo.faceType);
            }
        }
    }

    ObjectData GetPlayer()
    {
        var objectDataList = WorldManager.Instance.ObjectDataList;
        for (var i = 0; i < objectDataList.Count; i++)
        {
            var objData = objectDataList[i];
            var creatureStateData = objData.GetData<CreatureStateData>();
            if (creatureStateData != null && creatureStateData.campType == CreatureCampType.Player)
            {
                return objData;
            }
        }

        LogUtil.W("Failed to find player!");
        return null;
    }
}
