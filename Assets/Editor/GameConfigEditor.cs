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

    [MenuItem("Config/Create Panel Config")]
    static void CreatePanelConfig()
    {
        PanelConfig resourceConfig = ScriptableObject.CreateInstance<PanelConfig>();
        string path = "Assets/Resources/Config/PanelConfig.asset";
        AssetDatabase.CreateAsset(resourceConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Localization Config")]
    static void CreateLocalizationConfig()
    {
        LocalizationConfig localizationConfig = ScriptableObject.CreateInstance<LocalizationConfig>();
        string path = "Assets/Resources/Config/LocalizationConfig.asset";
        AssetDatabase.CreateAsset(localizationConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Localization Config Group")]
    static void CreateLocalizationConfigGroup()
    {
        LocalizationConfigGroup localizationConfigList = ScriptableObject.CreateInstance<LocalizationConfigGroup>();
        string path = "Assets/Resources/Config/LocalizationConfigGroup.asset";
        AssetDatabase.CreateAsset(localizationConfigList, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Resource Preload Config")]
    static void CreateResourcePreloadConfig()
    {
        ResourcePreloadConfig resourcePreloadConfig = ScriptableObject.CreateInstance<ResourcePreloadConfig>();
        string path = "Assets/Resources/Config/ResourcePreloadConfig.asset";
        AssetDatabase.CreateAsset(resourcePreloadConfig, path);
        AssetDatabase.SaveAssets();
    }
}
