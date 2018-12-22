using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [System.Serializable]
    public struct RoleInfoLite
    {
        public string roleId;
        public string roleName;
        public int roleLevel;
        public int serverId;
    }

    public struct RoleInfo
    {
        public string roleId;
        public string roleName;
        public int roleLevel;
        public int serverId;
    }

    public class PlayerBaseData : Data
    {
        public string accountId;
        public List<RoleInfoLite> roleInfoLiteList = new List<RoleInfoLite>();
        public RoleInfo roleInfo;
        public PlayerNotification notification = new PlayerNotification();
    }

    [System.Serializable]
    public struct ServerInfo
    {
        public int serverId;
        public string serverName;
        public string serverAddress;
        public int serverPort;
    }

    public class ServerData : Data
    {
        public List<ServerInfo> serverInfoList = new List<ServerInfo>();
        public ServerInfo serverInfo;
    }
}
