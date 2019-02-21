using FlatBuffers;
using System;

public enum PlayerNotificationType
{
    GetRoleInfoSuccess,
    GetRoleInfoFailed,
    CreateRoleSuccess,
    CreateRoleFailed
}

public class PlayerNotification : BaseNotification
{
    NotificationData notification;

    public PlayerNotification()
    {
        _id = Constant.NOTIFICATION_TYPE_NETWORK;
        _typeList = new int[] {(int)Protocols.ResRoleInfo, (int)Protocols.ResCreateRole };

        notification.id = Constant.NOTIFICATION_TYPE_PLAYER;

        Enabled = true;
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        var msg = (Message)notificationData;
        var byteBuffer = new ByteBuffer(msg.data);

        switch(type)
        {
            case (int)Protocols.ResRoleInfo:
                ResRoleInfo(byteBuffer);
                break;
            case (int)Protocols.ResCreateRole:
                ResCreateRole(byteBuffer);
                break;
        }
    }

    public enum GetRoleInfoResult
    {
        Success = 0,
    }

    void ResRoleInfo(ByteBuffer byteBuffer)
    {
        var worldMgr = WorldManager.Instance;
        var resRoleInfo = Protocol.Login.ResRoleInfo.GetRootAsResRoleInfo(byteBuffer);

        if (resRoleInfo.Result == (int)GetRoleInfoResult.Success)
        {
            var roleInfo = resRoleInfo.RoleInfo.Value;

            var playerBaseData = worldMgr.GameCore.GetData<Data.PlayerBaseData>();
            playerBaseData.roleInfo.roleId = roleInfo.RoleId;
            playerBaseData.roleInfo.roleName = roleInfo.RoleName;
            playerBaseData.roleInfo.roleLevel = roleInfo.RoleLevel;

            notification.type = (int)PlayerNotificationType.GetRoleInfoSuccess;
            notification.mode = NotificationMode.ValueType;
            notification.data2 = playerBaseData.roleInfo;
        }
        else
        {
            notification.type = (int)PlayerNotificationType.GetRoleInfoFailed;
            notification.mode = NotificationMode.ValueType;
            notification.data2 = resRoleInfo.Result;
        }

        worldMgr.NotificationCenter.Notificate(notification);
    }

    public enum CreateRoleResult
    {
        Success = 0,
    }

    void ResCreateRole(ByteBuffer byteBuffer)
    {
        var worldMgr = WorldManager.Instance;
        var resCreateRole = Protocol.Login.ResCreateRole.GetRootAsResCreateRole(byteBuffer);

        if (resCreateRole.Result == (int)CreateRoleResult.Success)
        {
            var roleInfo = resCreateRole.RoleInfo.Value;

            var playerBaseData = worldMgr.GameCore.GetData<Data.PlayerBaseData>();
            playerBaseData.roleInfo.roleId = roleInfo.RoleId;
            playerBaseData.roleInfo.roleName = roleInfo.RoleName;
            playerBaseData.roleInfo.roleLevel = roleInfo.RoleLevel;

            notification.type = (int)PlayerNotificationType.CreateRoleSuccess;
            notification.mode = NotificationMode.ValueType;
            notification.data2 = playerBaseData.roleInfo;
        }
        else
        {
            notification.type = (int)PlayerNotificationType.CreateRoleFailed;
            notification.mode = NotificationMode.ValueType;
            notification.data2 = resCreateRole.Result;
        }

        worldMgr.NotificationCenter.Notificate(notification);
    }
}
