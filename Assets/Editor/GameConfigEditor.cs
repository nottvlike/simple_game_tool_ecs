using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameConfigEditor {
    [MenuItem("Config/Create Game Config")]
    static void CreateGameConfig()
    {
        GameConfig gameConfig = ScriptableObject.CreateInstance<GameConfig>();//生成可编辑对象
        string path = "Assets/Resources/Config/GameConfig.asset";//保存的路径
        AssetDatabase.CreateAsset(gameConfig, path);//第一步
        AssetDatabase.SaveAssets();//第二步
    }

    [MenuItem("Config/Create Resource Config")]
    static void CreateResourceConfig()
    {
        ResourceConfig resourceConfig = ScriptableObject.CreateInstance<ResourceConfig>();//生成可编辑对象
        string path = "Assets/Resources/Config/ResourceConfig.asset";//保存的路径
        AssetDatabase.CreateAsset(resourceConfig, path);//第一步
        AssetDatabase.SaveAssets();//第二步
    }
}
