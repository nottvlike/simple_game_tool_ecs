using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ServerItem : MonoBehaviour 
{
    public Button selectButton;

    void Awake()
    {
        selectButton.onClick.AddListener(OnSelectClick);
    }

    void OnSelectClick()
    {

    }
}
