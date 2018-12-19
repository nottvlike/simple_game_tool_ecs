using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public struct GetServerInfoResult
{
    public int result;
    public List<Data.ServerInfo> serverInfoList;
}

public class ServerPanel : Panel 
{
    public GridLayoutGroup grid;
    public ServerItem serverItemTemplate;

    public Button closeButton;

    List<ServerItem> _serverItemList = new List<ServerItem>();

    protected override void OnInit()
    {
        closeButton.onClick.AddListener(OnCloseClick);
    }

    protected override void OnShow(params object[] args)
    {
        var serverData = WorldManager.Instance.Player.GetData<Data.ServerData>();
        var serverInfoList = serverData.serverInfoList;
        var selectServerInfo = serverData.serverInfo;
        for (var i = 0; i < serverInfoList.Count; i++)
        {
            var serverInfo = serverInfoList[i];
            ServerItem item;
            if (i < _serverItemList.Count)
            {
                item = _serverItemList[i];
            }
            else
            {
                item = Instantiate(serverItemTemplate);
                item.transform.localScale = serverItemTemplate.transform.localScale;
                item.transform.localRotation = serverItemTemplate.transform.localRotation;
                item.transform.SetParent(grid.transform);

                _serverItemList.Add(item);
            }

            item.Init(serverInfo);
            item.IsSelected = serverInfo.serverId == selectServerInfo.serverId;
            item.gameObject.SetActive(true);
        }

        for (var i = serverInfoList.Count; i < _serverItemList.Count; i++)
        {
            _serverItemList[i].gameObject.SetActive(false);
        }
    }

    void OnCloseClick()
    {
        WorldManager.Instance.UIMgr.HidePanel(PanelType);
    }
}
