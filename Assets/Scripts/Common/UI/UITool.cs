using UnityEngine;
using System;
using System.Collections.Generic;

public enum PanelNotificationType
{
    None,
    OpenPanel,
    ClosePanel
}

public delegate void OnRootLoadedFinished(PanelType panelType);

public class UITool : IUITool
{
    Dictionary<PanelType, Panel> _panelDataDict = new Dictionary<PanelType, Panel>();
    List<PanelType> _showedPanelList = new List<PanelType>();

    PanelData _defaultPanelConfig;

    GameObject _uiRoot;

    PanelType _lastShowedPanelType = PanelType.None;

    NotificationData _notificationData;

    public void Init()
    {
        _notificationData = new NotificationData();
        _notificationData.id = Constant.NOTIFICATION_TYPE_UI;
    }

    void LoadUIRoot(PanelType panelType, OnRootLoadedFinished onLoaded)
    {
        // 异步加载 UI
        WorldManager.Instance.ResourceMgr.LoadAsync("UI Root", delegate (UnityEngine.Object obj)
        {
            _uiRoot = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
            onLoaded(panelType);
        });
    }

    public PanelData GetPanelConfig(PanelType panelType)
    {
        PanelData config;
        var panelConfigDict = WorldManager.Instance.PanelConfig.PanelConfigDict;
        if (panelConfigDict.TryGetValue(panelType, out config))
        {
            return config;
        }
        else
        {
            LogUtil.W("Could not find PanelConfig {0}!", panelType.ToString());
        }

        return _defaultPanelConfig;
    }

    public void AddPanel(Panel data)
    {
        var panelType = data.PanelType;
        if (!_panelDataDict.ContainsKey(panelType))
        {
            _panelDataDict.Add(panelType, data);
        }
        else
        {
            LogUtil.W("PanelData {0} has been added!", data.PanelType.ToString());
        }
    }

    public void RemovePanel(Panel data)
    {
        var panelType = data.PanelType;
        if (_panelDataDict.ContainsKey(panelType))
        {
            _panelDataDict.Remove(panelType);
        }
        else
        {
            LogUtil.W("PanelData {0} is not exist!", data.PanelType.ToString());
        }
    }

    public Panel GetPanel(PanelType panelType)
    {
        Panel panel;
        if (!_panelDataDict.TryGetValue(panelType, out panel))
        {
            return null;
        }

        return panel;
    }

    public void ShowPanel(PanelType panelType, params object[] args)
    {
        if (_uiRoot == null)
        {
            LoadUIRoot(panelType, delegate {
                ShowPanel(panelType, args);
            });
            return;
        }

        if (_showedPanelList.IndexOf(panelType) != -1)
        {
            LogUtil.W("PanelData {0} has been showed!", panelType.ToString());
            return;
        }

        var panelConfig = GetPanelConfig(panelType);
        if (panelConfig.panelMode == PanelMode.Popover)
        {
            ShowPanelImpl(panelType, args);
            return;
        }

        // 保存最后打开面板
        UpdateLastShowedPanel();

        // 关闭即将打开面板的其它子面板
        for (var i = 0; i < _showedPanelList.Count;)
        {
            var showedPanelType = _showedPanelList[i];
            var showedPanelConfig = GetPanelConfig(showedPanelType);
            if (showedPanelConfig.rootPanelType == panelType)
            {
                HidePanelImpl(showedPanelType);
            }
            else
            {
                i++;
            }
        }

        ShowPanelImpl(panelType, args);
    }

    void ShowPanelImpl(PanelType panelType, params object[] args)
    {
        Panel data;
        if (_panelDataDict.TryGetValue(panelType, out data))
        {
            _showedPanelList.Add(panelType);
            data.Show(args);

            _notificationData.type = (int)PanelNotificationType.OpenPanel;
            _notificationData.mode = NotificationMode.Object;
            _notificationData.data1 = data;
            WorldManager.Instance.NotificationCenter.Notificate(_notificationData);
        }
        else
        {
            var panelConfig = GetPanelConfig(panelType);
            if (panelConfig.panelType != PanelType.None)
            {
                // 异步加载 UI
                WorldManager.Instance.ResourceMgr.LoadAsync(panelConfig.resourceName, delegate (UnityEngine.Object obj)
                {
                    var panel = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
                    var rectTransform = panel.GetComponent<RectTransform>();
                    rectTransform.SetParent(_uiRoot.transform);
                    rectTransform.offsetMax = Vector2.zero;
                    rectTransform.offsetMin = Vector2.zero;

                    ShowPanelImpl(panelType, args);
                });
            }
        }
    }

    public void HidePanel(PanelType panelType)
    {
        if (_uiRoot == null)
        {
            LoadUIRoot(panelType, delegate {
                HidePanel(panelType);
            });
            return;
        }

        if (_showedPanelList.IndexOf(panelType) == -1)
        {
            LogUtil.W("PanelData {0} is not showed!", panelType.ToString());
            return;
        }

        var panelConfig = GetPanelConfig(panelType);
        if (panelConfig.panelMode == PanelMode.Popover)
        {
            HidePanelImpl(panelType);
            return;
        }

        // 关闭即将关闭面板的所有子面板
        for (var i = 0; i < _showedPanelList.Count;)
        {
            var showedPanelType = _showedPanelList[i];
            var showedPanelConfig = GetPanelConfig(showedPanelType);
            if (showedPanelConfig.rootPanelType == panelType)
            {
                HidePanelImpl(showedPanelType);
            }
            else
            {
                i++;
            }
        }

        HidePanelImpl(panelType);
    }

    void HidePanelImpl(PanelType panelType)
    {
        Panel data;
        if (_panelDataDict.TryGetValue(panelType, out data))
        {
            _showedPanelList.Remove(panelType);
            data.Hide();

            _notificationData.type = (int)PanelNotificationType.ClosePanel;
            _notificationData.mode = NotificationMode.Object;
            _notificationData.data1 = data;
            WorldManager.Instance.NotificationCenter.Notificate(_notificationData);
        }
        else
        {
            LogUtil.W("Could not find PanelData {0}!", panelType.ToString());
        }
    }

    public void ShowLastShowedPanel(params object[] args)
    {
        if (_uiRoot == null)
        {
            LoadUIRoot(_lastShowedPanelType, delegate {
                ShowLastShowedPanel(args);
            });
            return;
        }

        if (_lastShowedPanelType == PanelType.None)
            return;

        ShowPanel(_lastShowedPanelType, args);
    }


    void UpdateLastShowedPanel()
    {
        _lastShowedPanelType = PanelType.None;

        var rootPanelType = PanelType.None;
        for (var i = 0; i < _showedPanelList.Count; i++)
        {
            var showedPanelType = _showedPanelList[i];
            var panelConfig = GetPanelConfig(showedPanelType);
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

    public void Destroy()
    {
        _panelDataDict.Clear();
        _showedPanelList.Clear();
    }
}
