using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum ResourceNotificationType
{
    None,
    InitFinished
}

public enum ResourceLoadStateType
{
    None,
    Loading,
    Finished
}

/// <summary>
/// 异步加载资源请求结构体
/// </summary>
public class AsyncResourceRequest : IPoolObject
{
    public string resourceName;
    public OnResourceLoadFinished callBack;

    public bool IsInUse
    {
        get;
        set;
    }

    public void Clear()
    {
        resourceName = string.Empty;
        callBack = null;
    }
}

public class ResourceTool : MonoSingleton<ResourceTool> , IResourceTool
{
	public const string PREFIX_ASSETBUNDLE_PATH = "AssetBundle";
	public const string SUFFIX_ASSETBUNDLE_PATH = ".assetbundle";

    public string[] PreloadResourceList;

    Dictionary<string, ResourceData> _resourceDataDict;
    Dictionary<string, Object> _resourceDict = new Dictionary<string, Object>();
    List<AsyncResourceRequest> _asyncResourceRequestList = new List<AsyncResourceRequest>();

    NotificationData _notificationData;

    ResourceLoadStateType _resourceLoadState = ResourceLoadStateType.None;
    ResourceLoadStateType ResourceLoadState 
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

        _notificationData = new NotificationData();
        _notificationData.id = Constant.NOTIFICATION_TYPE_RESOURCE;

        var count = 0;
        for (var i = 0; i < PreloadResourceList.Length; i++)
        {
            var resourceName = PreloadResourceList[i];
            LoadAsync(resourceName, delegate (Object obj) {
                count++;

                if (count == PreloadResourceList.Length)
                {
                    _notificationData.mode = NotificationMode.ValueType;
                    _notificationData.data2 = count;

                    WorldManager.Instance.NotificationCenter.Notificate(_notificationData);
                }
            });
        }
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
    public Object Load(string resourceName)
    {
        Object resource = null;
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
    /// <param name="callback">加载成功的回调</param>
	public void LoadAsync(string resourceName, OnResourceLoadFinished callback)
	{
        Object resource;
        if (_resourceDict.TryGetValue(resourceName, out resource))
        {
            LoadAsyncFinished(resource, callback);
            return;
        }

        for (var i = 0; i < _asyncResourceRequestList.Count; i++)
        {
            var request = _asyncResourceRequestList[i];
            if (request.resourceName == resourceName)
            {
                request.callBack += callback;
                return;
            }
        }

		var resourceRequest = WorldManager.Instance.PoolMgr.Get<AsyncResourceRequest>();
		resourceRequest.resourceName = resourceName;
		resourceRequest.callBack = callback;

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
        if (_resourceDataDict.TryGetValue(asyncRequest.resourceName, out resourceInfo))
        {
            StartCoroutine(LoadCoroutine(resourceInfo, asyncRequest.callBack));
        }
        else
        {
            LogUtil.W(string.Format("No resource named {0} found!", asyncRequest.resourceName));
        }

        WorldManager.Instance.PoolMgr.Release(asyncRequest);
        _asyncResourceRequestList.Remove(asyncRequest);
    }

    IEnumerator LoadCoroutine(ResourceData resourceInfo, OnResourceLoadFinished callback)
    {
        ResourceLoadState = ResourceLoadStateType.Loading;

        Object resource;

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

        LogUtil.I("Load Resource " + resourceInfo.resourceName);

        _resourceDict.Add(resourceInfo.resourceName, resource);

        LoadAsyncFinished(resource, callback);
    }

    void LoadAsyncFinished(Object resource, OnResourceLoadFinished callback)
    {
        if (callback != null)
        {
            callback(resource);
        }

        ResourceLoadState = ResourceLoadStateType.Finished;
    }

    public void Destroy()
    {
        _resourceDataDict.Clear();
        _asyncResourceRequestList.Clear();
        _resourceDict.Clear();
    }
}
