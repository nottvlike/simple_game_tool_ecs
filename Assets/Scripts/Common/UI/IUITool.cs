using UnityEngine;
using System.Collections;


public enum PanelMode
{
    None = 0,
    Alone,          //独占的独立面板
    Child,          //独立子面板
    Popover         //确认框等面板
}

public enum PanelGroup
{
    None = 0,
    Login,
    Main,
    Fight
}

public enum PanelType
{
    None = 0,
    GameUpdatePanel,
    LoginPanel,
    ServerPanel,
    CreateRolePanel,
    MainPanel,
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
    void ShowLastShowedPanel();

    void Destroy();
}
