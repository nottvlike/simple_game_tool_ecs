using FlatBuffers;
using System;

public class PlayerNotification : ValueTypeNotification
{
    public PlayerNotification()
    {
        _id = Constant.NOTIFICATION_TYPE_PLAYER;
        _typeList = new int[] { (int)Protocols.ResLoginGame };
    }

    public override void OnReceive(int type, ValueType notificationData)
    {
        var msg = (Message)notificationData;

        var byteBuffer = new ByteBuffer(msg.data);
        var resLoginGame = Protocol.Login.ResLoginGame.GetRootAsResLoginGame(byteBuffer);

        var playerBaseData = WorldManager.Instance.Player.GetData<Data.PlayerBaseData>();
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
    }
}
