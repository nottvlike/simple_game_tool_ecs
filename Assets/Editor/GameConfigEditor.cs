using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfigEditor {
    [MenuItem("Config/Create Game Config")]
    static void CreateGameConfig()
    {
        GameConfig gameConfig = ScriptableObject.CreateInstance<GameConfig>();
        string path = "Assets/Resources/Config/GameConfig.asset";
        AssetDatabase.CreateAsset(gameConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create ActorJoyStick Config")]
    static void CreateJoyStickConfig()
    {
        JoyStickConfig gameConfig = ScriptableObject.CreateInstance<JoyStickConfig>();
        string path = "Assets/Resources/Config/JoyStickConfig.asset";
        AssetDatabase.CreateAsset(gameConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Resource Config")]
    static void CreateResourceConfig()
    {
        ResourceConfig resourceConfig = ScriptableObject.CreateInstance<ResourceConfig>();
        string path = "Assets/Resources/Config/ResourceConfig.asset";
        AssetDatabase.CreateAsset(resourceConfig, path);
        AssetDatabase.SaveAssets();
    }
}
