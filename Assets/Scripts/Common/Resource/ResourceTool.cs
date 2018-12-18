/*
 * 对于一个prefab，将其相关的资源拆分为6个部分
 * 1，texture资源单独打一个assetbundle
 * 2, material资源单独打一个assetbundle
 * 3, shader资源单独打一个assetbundle
 * 4, animator控制器单独打一个assetbundle
 * 5，模型资源单独打一个assetbundle
 * 6，prefab单独打一个assetbundle
 *
 * 1~5称作共享assetbundle（或者依赖assetbundle）
 * 6称作主assetbundle
 * */

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using LitJson;

public enum ResourceLoadStateType {
    None,
    Loading,
    Finished
}

/// <summary>
/// 异步加载资源请求结构体
/// </summary>
public struct AsyncResourceRequest : IEquatable<AsyncResourceRequest>
{
    public string ResourceName;
    public OnResourceLoadFinished CallBack;

    public bool Equals(AsyncResourceRequest other)
    {
        return ResourceName == other.ResourceName;
    }
}

public class ResourceTool : MonoSingleton<ResourceTool> , IResourceTool
{
	public const string PREFIX_ASSETBUNDLE_PATH = "AssetBundle";
	public const string SUFFIX_ASSETBUNDLE_PATH = ".assetbundle";

    Dictionary<string, ResourceData> _resourceDataDict;

    Dictionary<string, UnityEngine.Object> _resourceDict = new Dictionary<string, UnityEngine.Object>();

    List<AsyncResourceRequest> _asyncResourceRequestList = new List<AsyncResourceRequest>();

    ResourceLoadStateType _resourceLoadState = ResourceLoadStateType.None;
    bool _isInit = false;

    public ResourceLoadStateType ResourceLoadState 
    {
        set
        {
            _resourceLoadState = value;

            if (_resourceLoadState == ResourceLoadStateType.Finished)
                StartLoadAsync();
        }
        get { return _resourceLoadState; }
    }

    public void Init()
    {
        if (_resourceDataDict != null)
        {
            LogUtil.W("Resource tool has inited!");
            return;
        }

        LoadResourceConfig();
    }

    void LoadResourceConfig()
    {
        var resourceConfig = Resources.Load("Config/ResourceConfig") as ResourceConfig;
        _resourceDataDict = resourceConfig.ResourceDict;
    }

    public bool IsResourceLoaded(string resourceName)
    {
        return _resourceDict.ContainsKey(resourceName);
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public UnityEngine.Object Load(string resourceName)
    {
        UnityEngine.Object resource = null;
        if (_resourceDict.TryGetValue(resourceName, out resource))
        {
            return resource;
        }

        ResourceData resourceInfo;
        if (_resourceDataDict.TryGetValue(resourceName, out resourceInfo))
        {
            resource = Resources.Load(resourceInfo.resourcePath);
            _resourceDict.Add(resourceName, resource);

            return resource;
        }
        else
        {
            LogUtil.W(string.Format("No resource named {0} found!", resourceName));
        }

        return null;
    }

    /// <summary>
    /// 异步加载资源
    /// </summary>
    /// <param name="resourceName">资源名称</param>
    /// <param name="func">加载成功的回调</param>
	public void LoadAsync(string resourceName, OnResourceLoadFinished func)
	{
		var resourceRequest = new AsyncResourceRequest();
		resourceRequest.ResourceName = resourceName;
		resourceRequest.CallBack = func;

		_asyncResourceRequestList.Add(resourceRequest);

        StartLoadAsync();
	}

	void  StartLoadAsync()
	{
		if (_asyncResourceRequestList.Count <= 0)
		{
			return;
		}
		
		if (_resourceLoadState == ResourceLoadStateType.Loading)
		{
			return;
		}
		
		var asyncRequest = _asyncResourceRequestList[0];

        ResourceData resourceInfo;
        if (_resourceDataDict.TryGetValue(asyncRequest.ResourceName, out resourceInfo))
        {
            StartCoroutine(LoadCoroutine(resourceInfo, asyncRequest.CallBack));
        }
        else
        {
            LogUtil.W(string.Format("No resource named {0} found!", asyncRequest.ResourceName));
        }

        _asyncResourceRequestList.Remove(asyncRequest);
    }

    IEnumerator LoadCoroutine(ResourceData resourceInfo, OnResourceLoadFinished callback)
    {
        ResourceLoadState = ResourceLoadStateType.Loading;

        UnityEngine.Object resource;

        if (resourceInfo.isFromAssetBundle)
        {
            var assetbundlePath = string.Format("{0}{1}/{2}{3}", Application.streamingAssetsPath, PREFIX_ASSETBUNDLE_PATH, resourceInfo.assetbundlePath, SUFFIX_ASSETBUNDLE_PATH);
            WWW www = new WWW(assetbundlePath);
            yield return www;

            var bundle = www.assetBundle;
            var request = bundle.LoadAssetAsync<GameObject>(resourceInfo.resourceName);
            yield return request;

            resource = request.asset;
            bundle.Unload(false);

        }
        else
        {
            var request = Resources.LoadAsync(resourceInfo.resourcePath);
            yield return request;
            resource = request.asset;
        }

        _resourceDict.Add(resourceInfo.resourceName, resource);

        if (callback != null)
        {
            callback(resource);
        }

        ResourceLoadState = ResourceLoadStateType.Finished;
    }

    public void Destroy()
    {

    }
}
