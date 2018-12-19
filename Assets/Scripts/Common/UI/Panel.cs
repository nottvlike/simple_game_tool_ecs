using System;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField]
    PanelType _panelType;
    [SerializeField]
    bool _inactiveWhenHide;
    [SerializeField]
    int _order;

    Canvas _canvas;

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

    public void Show(params object[] args)
    {
        if (IsOpen)
        {
            return;
        }

        transform.SetSiblingIndex(_order);

        gameObject.SetActive(true);

        OnShow(args);
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

    protected virtual void OnShow(params object[] args) {}
    protected virtual void OnHide() {}
}
