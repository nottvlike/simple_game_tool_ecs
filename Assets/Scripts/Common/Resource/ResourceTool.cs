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
    public float Id;
    public string ResourceName;
    public OnResourceLoadFinished CallBack;

    public bool Equals(AsyncResourceRequest other)
    {
        return Id == other.Id && ResourceName == other.ResourceName;
    }
}

public class ResourceTool : MonoSingleton<ResourceTool> , IResourceTool
{
	public string ConfigurationConfig = "Config/ConfigurationTest";
	public string AssetBundleConfig = "AssetBundleConfigTest";

	public const string PREFIX_RESOURCE_PATH = "Prefab";
	public const string PREFIX_ASSETBUNDLE_PATH = "AssetBundle";
	public const string SUFFIX_ASSETBUNDLE_PATH = ".assetbundle";

    Dictionary<string, ResourceData> _resourceDict;

    Dictionary<string, ResourceRequest> _resourceRequestDict = new Dictionary<string, ResourceRequest>();

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
        if (!_isInit)
        {
            _isInit = true;
            var resourceConfig = Load("ResourceConfig", "Config/ResourceConfig", "", false) as ResourceConfig;
            _resourceDict = resourceConfig.ResourceDict;
        }
    }

    public bool IsResourceLoaded(string resourceName)
    {
        ResourceRequest resourceRequest = null;
        if (!_resourceRequestDict.TryGetValue(resourceName, out resourceRequest))
        {
            return false;
        }

        if (resourceRequest.Resource == null )
        {
            return false;
        }

        return true;
    }

    UnityEngine.Object Load(string resourceName, string resourcePath, string assetbundlePath, bool isFromAssetbundle)
    {
        ResourceRequest resourceRequest = null;
        if (_resourceRequestDict.TryGetValue(resourceName, out resourceRequest))
        {
            resourceRequest.Load();
        }
        else
        {
            resourceRequest = new ResourceRequest();
            resourceRequest.Init(resourceName, resourcePath, assetbundlePath, isFromAssetbundle);
            resourceRequest.Load();

            _resourceRequestDict.Add(resourceName, resourceRequest);
        }

        return resourceRequest.Resource;
    }

    /// <summary>
    /// 同步加载资源
    /// </summary>
    /// <param name="resourceName"></param>
    /// <returns></returns>
    public UnityEngine.Object Load(string resourceName)
    {
        if (_resourceLoadState == ResourceLoadStateType.Loading)
        {
            Debug.Log("An resource is loading, Please wait!");
            return null;
        }

        ResourceRequest resourceRequest = null;
        if (_resourceRequestDict.TryGetValue(resourceName, out resourceRequest))
        {
            resourceRequest.Load();
        }
        else
        {
            ResourceData resourceInfo;
            if (_resourceDict.TryGetValue(resourceName, out resourceInfo))
            {
                resourceRequest = new ResourceRequest();
                resourceRequest.Init(resourceInfo);

                resourceRequest.Load();

                _resourceRequestDict.Add(resourceName, resourceRequest);
            }
            else
            {
                Debug.Log(string.Format("No resource named {0} found!", resourceName));
            }
        }
        return resourceRequest.Resource;
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
		resourceRequest.Id = Time.realtimeSinceStartup;
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
		var resourceName = asyncRequest.ResourceName;
		var callBack = asyncRequest.CallBack;
		
        _asyncResourceRequestList.Remove(asyncRequest);

        ResourceRequest resourceRequest = null;
        if (_resourceRequestDict.TryGetValue(resourceName, out resourceRequest))
        {
            resourceRequest.LoadAsync(callBack);
        }
        else
        {
            ResourceData resourceInfo;
            if (_resourceDict.TryGetValue(resourceName, out resourceInfo))
            {
                resourceRequest = new ResourceRequest();
                resourceRequest.Init(resourceInfo);

                resourceRequest.LoadAsync(callBack);

                _resourceRequestDict.Add(resourceName, resourceRequest);
            }
            else
            {
                Debug.Log(string.Format("No resource named {0} found!", resourceName));
            }
        }
    }

    public void Destroy()
    {

    }
}
