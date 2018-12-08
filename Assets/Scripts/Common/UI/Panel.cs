using System;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField]
    PanelType _panelType;
    [SerializeField]
    bool _inactiveWhenHide;

    public bool IsOpen
    {
        get { return gameObject.activeSelf; }
    }

    public PanelType PanelType
    {
        get { return _panelType; }
    }

    public bool InactiveWhenHide
    {
        get { return _inactiveWhenHide; }
    }

    // Use this for initialization
    void Awake()
    {
        WorldManager.Instance.UIMgr.AddPanel(this);
        gameObject.SetActive(false);

        OnInit();
    }

    protected virtual void OnInit() {}

    public void Show()
    {
        if (IsOpen)
        {
            return;
        }

        gameObject.SetActive(true);

        OnShow();
    }

    public void Hide()
    {
        if (!IsOpen)
        {
            return;
        }

        if (_inactiveWhenHide)
        {
            gameObject.SetActive(false);
        }

        OnHide();
    }

    protected virtual void OnShow() {}
    protected virtual void OnHide() {}
}
