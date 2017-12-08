using System;
using UnityEngine;

public class Panel : MonoBehaviour, IPanel
{
    public PanelType panelType;

    public bool IsOpen
    {
        get { return gameObject.activeSelf; }
    }

    public PanelType PanelType
    {
        get { return panelType; }
    }

    public GameObject Resource
    {
        get { return gameObject; }
    }

    // Use this for initialization
    void Awake()
    {
        //UITool.Instance.AddPanel(this);
        gameObject.SetActive(false);
    }

    void OnDestroy()
    {
        //UITool.Instance.RemovePanel(this);
    }
}
