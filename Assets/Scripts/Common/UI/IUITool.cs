using UnityEngine;
using System.Collections;


public enum PanelMode
{
    None,
    Alone,          //独占的独立面板
    Child,          //独立子面板
    Popover         //确认框等面板
}

public enum PanelGroup
{
    None,
    Alone,
    Popover
}

public enum PanelType
{
    None,
    UpdatePanel,
    LoginPanel,
    CreateRolePanel,
    HomePanel,
    FightPanel
}

public interface IUITool
{
    void Init();

    void AddPanel(IPanel panel);
    void RemovePanel(IPanel panel);

    IPanel GetPanel(PanelType panelType);
    void ShowPanel(PanelType panelType);
    void HidePanel(PanelType panelType);

    void Destroy();
}
