using UnityEngine;

public interface IPanel
{
    PanelType PanelType { get; }
    bool IsOpen { get; }
    GameObject Resource { get; }
}
