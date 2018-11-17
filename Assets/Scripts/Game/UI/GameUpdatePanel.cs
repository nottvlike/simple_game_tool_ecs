using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameUpdatePanel : MonoBehaviour
{
    public Button skipButton;

    void Awake()
    {
        skipButton.onClick.AddListener(OnSkipClick);
    }

    void OnSkipClick()
    {
        WorldManager.Instance.UIMgr.ShowPanel(PanelType.LoginPanel);
    }
}
