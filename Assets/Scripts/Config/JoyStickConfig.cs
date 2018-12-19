using System;
using UnityEngine;
using Data;

public enum KeyStateType
{
    None = 0,
    Down,
    Up
}

[Serializable]
public struct JoyStickMapInfo
{
    public KeyCode[] keyCode;
    public KeyStateType keyStateType;
    public JoyStickActionType joyStickActionType;
    public JoyStickActionFaceType joyStickActionFaceType;
}

public class JoyStickConfig : ScriptableObject
{
    public JoyStickMapInfo[] joyStickMapDataList;
}


