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
    List<Module.Module> _moduleList = new List<Module.Module>();

    public List<ObjectData> ObjectDataList
    {
        get { return _objectDataList; }
    }

    public List<Module.Module> ModuleList
    {
        get { return _moduleList; }
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

    public Module.Module GetModule<T>()
    {
        var moduleType = typeof(T);
        for (var i = 0; i < _moduleList.Count; i++)
        {
            var module = _moduleList[i];
            if (moduleType == module.GetType())
                return module;
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

        GetSocketTool().SendMessage(buf, 10000, 10001, delegate (Message msg)
        {
            
        });

        LogUtil.I("buf " + buf.Length);

        LoadConfig();

        GetUITool().ShowPanel(PanelType.GameUpdatePanel);
        GetLevelLoader().DoDrama();
    }
}
