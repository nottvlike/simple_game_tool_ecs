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

        public override void Clear()
        {
            accountId = string.Empty;
            roleInfoLiteList.Clear();

            roleInfo.roleId = string.Empty;
            roleInfo.roleName = string.Empty;
            roleInfo.roleLevel = 0;
            roleInfo.serverId = 0;

            notification = null;
        }
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

        public override void Clear()
        {
            serverInfoList.Clear();

            serverInfo.serverId = 0;
            serverInfo.serverName = string.Empty;
            serverInfo.serverAddress = string.Empty;
            serverInfo.serverPort = 0;
        }
    }
}
