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
    Dictionary<PanelType, IPanel> _panelDataDict = new Dictionary<PanelType, IPanel>();
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
        OnResourceLoadFinished onFinished = delegate (UnityEngine.Object obj) {
            _uiRoot = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
            onLoaded(panelType);
        };

        // 异步加载 UI
        WorldManager.Instance.ResourceMgr.LoadAsync("UI Root", onFinished);
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

    public void RemovePanel(IPanel data)
    {
        if (_uiRoot == null)
        {
            LoadUIRoot(data.PanelType, delegate {
                RemovePanel(data);
            });
            return;
        }

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

    public IPanel GetPanel(PanelType panelType)
    {
        IPanel panel;
        if (!_panelDataDict.TryGetValue(panelType, out panel))
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

        if (_showedPanelList.IndexOf(panelType) != -1)
        {
            LogUtil.W("PanelData {0} has been showed!", panelType.ToString());
            return;
        }

        UpdateLastShowedPanel();

        var rootPanelType = panelConfig.rootPanelType;
        if (_showedPanelList.IndexOf(rootPanelType) != -1)
        {
            for (var i = _showedPanelList.Count - 1; i >= 0; i--)
            {
                var showedPanelType = _showedPanelList[i];
                if (showedPanelType == rootPanelType)
                {
                    continue;
                }

                HidePanel(showedPanelType);
                _showedPanelList.RemoveAt(i);
            }
        }
        else
        {
            for (var i = 0; i < _showedPanelList.Count; i++)
            {
                HidePanel(_showedPanelList[i]);
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

        if (_showedPanelList.IndexOf(panelType) == -1)
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

    void ShowPanelImpl(PanelType panelType)
    {
        IPanel data;
        if (_panelDataDict.TryGetValue(panelType, out data))
        {
            _showedPanelList.Add(panelType);
            data.Resource.SetActive(true);

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
                OnResourceLoadFinished onFinished = delegate (UnityEngine.Object obj) {
                    var panel = UnityEngine.Object.Instantiate(obj, Vector3.zero, Quaternion.identity) as GameObject;
                    var rectTransform = panel.GetComponent<RectTransform>();
                    rectTransform.SetParent(_uiRoot.transform);
                    rectTransform.offsetMax = Vector2.zero;
                    rectTransform.offsetMin = Vector2.zero;

                    ShowPanelImpl(panelType);
                };

                // 异步加载 UI
                WorldManager.Instance.ResourceMgr.LoadAsync(panelConfig.resourceName, onFinished);
            }
        }
    }

    void HidePanelImpl(PanelType panelType)
    {
        IPanel data;
        if (_panelDataDict.TryGetValue(panelType, out data))
        {
            _showedPanelList.Remove(panelType);

            data.Resource.SetActive(false);

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

    public void Destroy()
    {
    }
}
