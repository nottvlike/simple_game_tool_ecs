using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfigEditor
{
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

    [MenuItem("Config/Create Actor Config")]
    static void CreateActorConfig()
    {
        ActorConfig actorConfig = ScriptableObject.CreateInstance<ActorConfig>();
        string path = "Assets/Resources/Config/ActorConfig.asset";
        AssetDatabase.CreateAsset(actorConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Preload Config Group")]
    static void CreatePreloadConfigGroup()
    {
        ConfigGroup preloadConfigGroup = ScriptableObject.CreateInstance<ConfigGroup>();
        string path = "Assets/Resources/Config/Group/PreloadConfigGroup.asset";
        AssetDatabase.CreateAsset(preloadConfigGroup, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Main Config Group")]
    static void CreateMainConfigGroup()
    {
        ConfigGroup mainConfigGroup = ScriptableObject.CreateInstance<ConfigGroup>();
        string path = "Assets/Resources/Config/Group/MainConfigGroup.asset";
        AssetDatabase.CreateAsset(mainConfigGroup, path);
        AssetDatabase.SaveAssets();
    }
}
