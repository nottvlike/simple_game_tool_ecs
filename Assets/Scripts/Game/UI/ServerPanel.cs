using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerPanel : MonoBehaviour 
{
    public Button closeButton;

    void Awake()
    {
        closeButton.onClick.AddListener(OnCloseClick);
    }

    void OnCloseClick()
    {
        WorldManager.Instance.GetUITool().HidePanel(PanelType.ServerPanel);
    }
}
