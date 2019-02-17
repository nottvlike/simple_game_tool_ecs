using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfigEditor
{
    [MenuItem("Config/Create ActorJoyStick Config")]
    static void CreateJoyStickConfig()
    {
        var gameConfig = ScriptableObject.CreateInstance<JoyStickConfig>();
        string path = "Assets/Resources/Config/JoyStickConfig.asset";
        AssetDatabase.CreateAsset(gameConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Resource Config")]
    static void CreateResourceConfig()
    {
        var resourceConfig = ScriptableObject.CreateInstance<ResourceConfig>();
        string path = "Assets/Resources/Config/ResourceConfig.asset";
        AssetDatabase.CreateAsset(resourceConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Panel Config")]
    static void CreatePanelConfig()
    {
        var resourceConfig = ScriptableObject.CreateInstance<PanelConfig>();
        string path = "Assets/Resources/Config/PanelConfig.asset";
        AssetDatabase.CreateAsset(resourceConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Localization Config")]
    static void CreateLocalizationConfig()
    {
        var localizationConfig = ScriptableObject.CreateInstance<LocalizationConfig>();
        string path = "Assets/Resources/Config/LocalizationConfig.asset";
        AssetDatabase.CreateAsset(localizationConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Localization Config Group")]
    static void CreateLocalizationConfigGroup()
    {
        var localizationConfigList = ScriptableObject.CreateInstance<LocalizationConfigGroup>();
        string path = "Assets/Resources/Config/LocalizationConfigGroup.asset";
        AssetDatabase.CreateAsset(localizationConfigList, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Resource Preload Config")]
    static void CreateResourcePreloadConfig()
    {
        var resourcePreloadConfig = ScriptableObject.CreateInstance<ResourcePreloadConfig>();
        string path = "Assets/Resources/Config/ResourcePreloadConfig.asset";
        AssetDatabase.CreateAsset(resourcePreloadConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Actor Config")]
    static void CreateActorConfig()
    {
        var actorConfig = ScriptableObject.CreateInstance<ActorConfig>();
        string path = "Assets/Resources/Config/ActorConfig.asset";
        AssetDatabase.CreateAsset(actorConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Preload Config Group")]
    static void CreatePreloadConfigGroup()
    {
        var preloadConfigGroup = ScriptableObject.CreateInstance<ConfigGroup>();
        string path = "Assets/Resources/Config/PreloadConfigGroup.asset";
        AssetDatabase.CreateAsset(preloadConfigGroup, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Main Config Group")]
    static void CreateMainConfigGroup()
    {
        var mainConfigGroup = ScriptableObject.CreateInstance<ConfigGroup>();
        string path = "Assets/Resources/Config/MainConfigGroup.asset";
        AssetDatabase.CreateAsset(mainConfigGroup, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Buff Config")]
    static void CreateBuffConfig()
    {
        var buffConfig = ScriptableObject.CreateInstance<BuffConfig>();
        string path = "Assets/Resources/Config/BuffConfig.asset";
        AssetDatabase.CreateAsset(buffConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Battle Config")]
    static void CreateBattleConfig()
    {
        var battleConfig = ScriptableObject.CreateInstance<BattleConfig>();
        string path = "Assets/Resources/Config/BattleConfig.asset";
        AssetDatabase.CreateAsset(battleConfig, path);
        AssetDatabase.SaveAssets();
    }

    [MenuItem("Config/Create Battle Item Config")]
    static void CreateBattleItemConfig()
    {
        var battleItemConfig = ScriptableObject.CreateInstance<BattleItemConfig>();
        string path = "Assets/Resources/Config/BattleItemConfig.asset";
        AssetDatabase.CreateAsset(battleItemConfig, path);
        AssetDatabase.SaveAssets();
    }
}
