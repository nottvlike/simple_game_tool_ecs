using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCoeNotification : ValueTypeNotification
{
    public GameCoeNotification()
    {
        _id = Constant.NOTIFICATION_TYPE_GAME_CORE;
        _typeList = new int[] { (int)Protocols.ResHeartBeat };
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
    }
}
