using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public delegate void ButtonClickCallback();

public class YesNoPanel : Panel
{
    public Text Title;
    public Text Content;
    public Button okButton;
    public Text okText;
    public Button cancelButton;
    public Text cancelText;

    ButtonClickCallback _okCallback;
    ButtonClickCallback _cancelCallback;

    protected override void OnInit()
    {
        okButton.onClick.AddListener(OnOkClick);
        cancelButton.onClick.AddListener(OnCancelClick);
    }

    protected override void OnShow(params object[] args)
    {
        Title.text = args[0] as string;
        Content.text = args[1] as string;

        okText.text = args[2] as string;
        cancelText.text = args[3] as string;

        _okCallback = args[4] as ButtonClickCallback;
        _cancelCallback = args[5] as ButtonClickCallback;

        var isHideCancel = (bool)args[6];
        cancelButton.gameObject.SetActive(!isHideCancel);
    }

    void OnOkClick()
    {
        if (_okCallback != null)
        {
            _okCallback();
        }

        WorldManager.Instance.UIMgr.HidePanel(PanelType.YesNoPanel);
    }

    void OnCancelClick()
    {
        if (_cancelCallback != null)
        {
            _cancelCallback();
        }

        WorldManager.Instance.UIMgr.HidePanel(PanelType.YesNoPanel);
    }
}
