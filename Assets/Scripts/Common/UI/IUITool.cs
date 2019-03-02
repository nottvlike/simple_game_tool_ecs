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
    RoleSelectPanel,
    ServerPanel,
    CreateRolePanel,
    MainPanel,
    FightPanel,
    FightResultPanel,

    AsyncPanel = 1000,
    SimpleTipsPanel,
    YesNoPanel
}

public interface IUITool
{
    void Init();

    void AddPanel(Panel panel);
    void RemovePanel(Panel panel);

    Panel GetPanel(PanelType panelType);
    void ShowPanel(PanelType panelType, params object[] args);
    void HidePanel(PanelType panelType);
    void ShowLastShowedPanel(params object[] args);

    void LoadPanel(Object obj);

    void Destroy();
}
