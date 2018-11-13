using System;
using System.Collections.Generic;

public enum SystemType
{
    None,
}

public struct SystemInfo : IEquatable<SystemInfo>
{
    SystemType systemType;
    PanelType panelType;

    public bool Equals(SystemInfo other)
    {
        return systemType == other.systemType;
    }
}

public partial class WorldManager : Singleton<WorldManager>
{
    List<ObjectData> _objectDataList = new List<ObjectData>();

    public List<ObjectData> ObjectDataList
    {
        get { return _objectDataList; }
    }

    public ObjectData GetObjectData(int objId)
    {
        for (var i = 0; i < _objectDataList.Count; i++)
        {
            var objectData = _objectDataList[i];
            if (objId == objectData.ObjectId)
                return objectData;
        }

        return null;
    }

    public void LaunchGame()
    {
        GetResourceTool().Init();

        GetSocketTool().Init("127.0.0.1", 8888);

        RegisterAllModule();

        var builder = new FlatBuffers.FlatBufferBuilder(1024);
        var name = builder.CreateString("Test123");
        var password = builder.CreateString("123");
        var channelName = builder.CreateString("unity");

        Protocol.ReqLoginGame.StartReqLoginGame(builder);
        Protocol.ReqLoginGame.AddName(builder, name);
        Protocol.ReqLoginGame.AddPassword(builder, password);
        Protocol.ReqLoginGame.AddChannel(builder, 0);
        Protocol.ReqLoginGame.AddSubChannel(builder, 1);
        Protocol.ReqLoginGame.AddChannelName(builder, channelName);
        var orc = Protocol.ReqLoginGame.EndReqLoginGame(builder);
        builder.Finish(orc.Value);

        var buf = builder.SizedByteArray();

        GetSocketTool().SendMessage(buf, (int)Protocols.ReqLoginGame, (int)Protocols.ResLoginGame, delegate (Message msg)
        {
            var byteBuffer = new FlatBuffers.ByteBuffer(msg.data);
            var resLoginGame = Protocol.ResLoginGame.GetRootAsResLoginGame(byteBuffer);

            LogUtil.I("Result " + resLoginGame.Result);
            LogUtil.I("AccountId " + resLoginGame.AccountId);
            LogUtil.I("RoleInfoLitesLength " + resLoginGame.RoleInfoLitesLength);
            var roleInfoLite0 = resLoginGame.RoleInfoLites(0);
            LogUtil.I("RoleInfoLites(0) RoleId " + roleInfoLite0.Value.RoleId);
            LogUtil.I("RoleInfoLites(0) RoleName " + roleInfoLite0.Value.RoleName);
            LogUtil.I("RoleInfoLites(0) RoleLevel " + roleInfoLite0.Value.RoleLevel);
            var roleInfoLite1 = resLoginGame.RoleInfoLites(1);
            LogUtil.I("RoleInfoLites(1) RoleId " + roleInfoLite1.Value.RoleId);
            LogUtil.I("RoleInfoLites(1) RoleName " + roleInfoLite1.Value.RoleName);
            LogUtil.I("RoleInfoLites(1) RoleLevel " + roleInfoLite1.Value.RoleLevel);
            var roleInfoLite2 = resLoginGame.RoleInfoLites(2);
            LogUtil.I("RoleInfoLites(2) RoleId " + roleInfoLite2.Value.RoleId);
            LogUtil.I("RoleInfoLites(2) RoleName " + roleInfoLite2.Value.RoleName);
            LogUtil.I("RoleInfoLites(2) RoleLevel " + roleInfoLite2.Value.RoleLevel);
        });

        LogUtil.I("buf " + buf.Length);

        LoadConfig();

        GetUITool().ShowPanel(PanelType.GameUpdatePanel);
        GetLevelLoader().DoDrama();
    }
}
