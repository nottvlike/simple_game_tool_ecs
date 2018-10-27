using System;
using System.Collections.Generic;

public enum PanelNotificationType
{
    None,
    OpenPanel,
    ClosePanel
}

public struct PanelConfig : IEquatable<PanelConfig>
{
    public PanelType panelType;
    public PanelMode panelMode;
    public PanelGroup panelGroup;
    public PanelType rootPanelType;
    public string resourceName;

    public bool Equals(PanelConfig other)
    {
        return panelType == other.panelType;
    }
}

public class UITool : IUITool
{
    Dictionary<int, PanelConfig> _panelConfigDict = new Dictionary<int, PanelConfig>();
    Dictionary<int, IPanel> _panelDataDict = new Dictionary<int, IPanel>();
    List<int> _showedPanelList = new List<int>();

    PanelConfig _defaultPanelConfig;

    PanelType _lastShowedPanelType = PanelType.None;

    public void Init()
    {
    }

    public PanelConfig GetPanelConfig(PanelType panelType)
    {
        PanelConfig config;
        if (_panelConfigDict.TryGetValue((int)panelType, out config))
        {
            return config;
        }
        else
        {
            LogUtil.W("Could not find PanelConfig {0}!", panelType.ToString());
        }

        return _defaultPanelConfig;
    }

    public void AddPanel(IPanel data)
    {
        int panelType = (int)data.PanelType;
        if (!_panelDataDict.ContainsKey(panelType))
        {
            _panelDataDict.Add(panelType, data);
        }
        else
        {
            LogUtil.W("PanelData {0} has been added!", data.PanelType.ToString());
        }
    }

    public void RemovePanel(IPanel data)
    {
        int panelType = (int)data.PanelType;
        if (_panelDataDict.ContainsKey(panelType))
        {
            _panelDataDict.Remove(panelType);
        }
        else
        {
            LogUtil.W("PanelData {0} is not exist!", data.PanelType.ToString());
        }
    }

    public IPanel GetPanel(PanelType panelType)
    {
        IPanel panel;
        if (!_panelDataDict.TryGetValue((int)panelType, out panel))
        {
            return null;
        }

        return panel;
    }

    public void ShowPanel(PanelType panelType)
    {
        var panelConfig = GetPanelConfig(panelType);
        if (panelConfig.panelMode == PanelMode.Popover)
        {
            ShowPanelImpl(panelType);
            return;
        }

        if (_showedPanelList.IndexOf((int)panelType) != -1)
        {
            LogUtil.W("PanelData {0} has been showed!", panelType.ToString());
            return;
        }

        UpdateLastShowedPanel();

        var rootPanelType = panelConfig.rootPanelType;
        if (_showedPanelList.IndexOf((int)rootPanelType) != -1)
        {
            for (var i = _showedPanelList.Count - 1; i >= 0; i--)
            {
                var showedPanelType = _showedPanelList[i];
                if (showedPanelType == (int)rootPanelType)
                {
                    continue;
                }

                HidePanel((PanelType)showedPanelType);
                _showedPanelList.RemoveAt(i);
            }
        }
        else
        {
            for (var i = 0; i < _showedPanelList.Count; i++)
            {
                HidePanel((PanelType)_showedPanelList[i]);
            }

            _showedPanelList.Clear();

            if (rootPanelType != PanelType.None)
            {
                ShowPanelImpl(rootPanelType);
            }
        }

        ShowPanelImpl(panelType);
    }

    public void HidePanel(PanelType panelType)
    {
        var panelConfig = GetPanelConfig(panelType);
        if (panelConfig.panelMode == PanelMode.Popover)
        {
            HidePanelImpl(panelType);
            return;
        }

        if (_showedPanelList.IndexOf((int)panelType) == -1)
        {
            LogUtil.W("PanelData {0} is not showed!", panelType.ToString());
            return;
        }

        HidePanelImpl(panelType);
    }

    public void ShowLastShowedPanel()
    {
        if (_lastShowedPanelType == PanelType.None)
            return;

        ShowPanel(_lastShowedPanelType);
    }


    void UpdateLastShowedPanel()
    {
        var rootPanelType = PanelType.None;
        for (var i = 0; i < _showedPanelList.Count; i++)
        {
            var showedPanelType = _showedPanelList[i];
            var panelConfig = GetPanelConfig((PanelType)showedPanelType);
            if (panelConfig.panelMode == PanelMode.Alone)
            {
                rootPanelType = panelConfig.panelType;
            }

            if (panelConfig.panelMode == PanelMode.Child)
            {
                _lastShowedPanelType = panelConfig.panelType;
            }
        }

        if (_lastShowedPanelType == PanelType.None)
        {
            _lastShowedPanelType = rootPanelType;
        }
    }

    void ShowPanelImpl(PanelType panelType)
    {
        _showedPanelList.Add((int)panelType);

        IPanel data;
        if (_panelDataDict.TryGetValue((int)panelType, out data))
        {
            data.Resource.SetActive(true);

            var notificationData = new NotificationData();
            notificationData.id = Constant.NOTIFICATION_TYPE_UI;
            notificationData.type = (int)PanelNotificationType.OpenPanel;
            notificationData.mode = NotificationMode.Object;
            notificationData.state = NotificationStateType.None;
            notificationData.data1 = data;
            WorldManager.Instance.GetNotificationCenter().Notificate(notificationData);
        }
        else
        {
            // 异步加载 UI
        }
    }

    void HidePanelImpl(PanelType panelType)
    {
        IPanel data;
        if (_panelDataDict.TryGetValue((int)panelType, out data))
        {
            data.Resource.SetActive(false);

            var notificationData = new NotificationData();
            notificationData.id = Constant.NOTIFICATION_TYPE_UI;
            notificationData.type = (int)PanelNotificationType.ClosePanel;
            notificationData.mode = NotificationMode.Object;
            notificationData.state = NotificationStateType.None;
            notificationData.data1 = data;
            WorldManager.Instance.GetNotificationCenter().Notificate(notificationData);
        }
        else
        {
            LogUtil.W("Could not find PanelData {0}!", panelType.ToString());
        }
    }

    public void Destroy()
    {
    }
}
