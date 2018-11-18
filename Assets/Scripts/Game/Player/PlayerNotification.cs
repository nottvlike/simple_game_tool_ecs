using FlatBuffers;
using System;

public enum PlayerNotificationType
{
    OnLoginSuccess,
}

public class PlayerNotification : ValueTypeNotification
{
    NotificationData notification;

    public PlayerNotification()
    {
        _id = Constant.NOTIFICATION_TYPE_NETWORK;
        _typeList = new int[] { (int)Protocols.ResLoginGame };

        notification = new NotificationData();
        notification.id = Constant.NOTIFICATION_TYPE_PLAYER;
        notification.mode = NotificationMode.Object;

        Enabled = true;
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        var msg = (Message)notificationData;

        var byteBuffer = new ByteBuffer(msg.data);
        var resLoginGame = Protocol.Login.ResLoginGame.GetRootAsResLoginGame(byteBuffer);

        var worldMgr = WorldManager.Instance;
        var playerBaseData = worldMgr.Player.GetData<Data.PlayerBaseData>();
        playerBaseData.accountId = resLoginGame.AccountId;

        playerBaseData.roleInfoLiteList.Clear();
        for (var i = 0; i < resLoginGame.RoleInfoLitesLength; i++)
        {
            var roleInfoLite = new Data.RoleInfoLite();
            var roleInfoLiteTable = resLoginGame.RoleInfoLites(i);

            roleInfoLite.roleId = roleInfoLiteTable.Value.RoleId;
            roleInfoLite.roleName = roleInfoLiteTable.Value.RoleName;
            roleInfoLite.roleLevel = roleInfoLiteTable.Value.RoleLevel;

            playerBaseData.roleInfoLiteList.Add(roleInfoLite);
        }

        notification.type = (int)PlayerNotificationType.OnLoginSuccess;
        notification.data1 = playerBaseData;
        worldMgr.NotificationCenter.Notificate(notification);
    }
}
