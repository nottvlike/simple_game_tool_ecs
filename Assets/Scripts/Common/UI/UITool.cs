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
    Dictionary<int, IPanel> _panelDataDict = new Dictionary<int, IPanel>();
    List<int> _showedPanelList = new List<int>();

    PanelData _defaultPanelConfig;

    GameObject _uiRoot;

    PanelType _lastShowedPanelType = PanelType.None;

    public void Init()
    {
#if UNITOR_EDITOR
        var notificationCenter = WorldManager.Instance.GetNotificationCenter();
        notificationCenter.Register(Constant.NOTIFICATION_TYPE_UI, (int)PanelNotificationType.OpenPanel, NotificationMode.Object);
        notificationCenter.Register(Constant.NOTIFICATION_TYPE_UI, (int)PanelNotificationType.ClosePanel, NotificationMode.Object);
#endif
    }

    void LoadUIRoot(PanelType panelType, OnRootLoadedFinished onLoaded)
    {
        OnResourceLoadFinished onFinished = delegate (UnityEngine.Object obj) {
            _uiRoot = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
            onLoaded(panelType);
        };

        // 异步加载 UI
        WorldManager.Instance.GetResourceTool().LoadAsync("UI Root", onFinished);
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

    public void AddPanel(IPanel data)
    {
        if (_uiRoot == null)
        {
            LoadUIRoot(data.PanelType, delegate {
                AddPanel(data);
            });
            return;
        }

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
        if (_uiRoot == null)
        {
            LoadUIRoot(data.PanelType, delegate {
                RemovePanel(data);
            });
            return;
        }

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
        if (_uiRoot == null)
        {
            LoadUIRoot(panelType, delegate {
                ShowPanel(panelType);
            });
            return;
        }

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
        if (_uiRoot == null)
        {
            LoadUIRoot(panelType, delegate {
                HidePanel(panelType);
            });
            return;
        }

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
        if (_uiRoot == null)
        {
            LoadUIRoot(_lastShowedPanelType, delegate {
                ShowLastShowedPanel();
            });
            return;
        }

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
        IPanel data;
        if (_panelDataDict.TryGetValue((int)panelType, out data))
        {
            _showedPanelList.Add((int)panelType);
            data.Resource.SetActive(true);

            var notificationData = Constant.defaultNotificationData;
            notificationData.type = Constant.NOTIFICATION_TYPE_UI;
            notificationData.subType = (int)PanelNotificationType.OpenPanel;
            notificationData.mode = NotificationMode.Object;
            notificationData.state = NotificationStateType.None;
            notificationData.data1 = data;
            WorldManager.Instance.GetNotificationCenter().Notificate(notificationData);
        }
        else
        {
            var panelConfig = GetPanelConfig(panelType);
            if (panelConfig.panelType != PanelType.None)
            {
                OnResourceLoadFinished onFinished = delegate (UnityEngine.Object obj) {
                    var panel = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
                    var rectTransform = panel.GetComponent<RectTransform>();
                    rectTransform.parent = _uiRoot.transform;
                    rectTransform.offsetMax = Vector2.zero;
                    rectTransform.offsetMin = Vector2.zero;

                    ShowPanelImpl(panelType);
                };

                // 异步加载 UI
                WorldManager.Instance.GetResourceTool().LoadAsync(panelConfig.resourceName, onFinished);
            }
        }
    }

    void HidePanelImpl(PanelType panelType)
    {
        IPanel data;
        if (_panelDataDict.TryGetValue((int)panelType, out data))
        {
            data.Resource.SetActive(false);

            var notificationData = Constant.defaultNotificationData;
            notificationData.type = Constant.NOTIFICATION_TYPE_UI;
            notificationData.subType = (int)PanelNotificationType.ClosePanel;
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
