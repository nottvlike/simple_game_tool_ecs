using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HomePanel : MonoBehaviour {
    public Button fight;

    void Awake()
    {
        fight.onClick.AddListener(OnFightClick);
    }

    void OnFightClick()
    {
        WorldManager.Instance.GetUITool().ShowPanel(PanelType.FightPanel);
    }
}
