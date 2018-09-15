using UnityEngine;
using System.Collections;

public class ResourceRequest
{
    ResourceData _resourceInfo;

    OnResourceLoadFinished _callBack = null;
    Object _resource = null;

    public ResourceData ResourceInfo
    {
        get
        {
            return _resourceInfo;
        }
    }

    public Object Resource
    {
        get
        {
            return _resource;
        }
    }

    public void Init(ResourceData resourceInfo)
    {
        _resourceInfo = resourceInfo;
    }

    public void Init(string resourceName, string resourcePath, string assetbundlePath, bool isFromAssetBundle)
    {
        _resourceInfo.resourceName = resourceName;
        _resourceInfo.resourcePath = resourcePath;
        _resourceInfo.assetbundlePath = assetbundlePath;
        _resourceInfo.isFromAssetBundle = isFromAssetBundle;
    }

    public void LoadAsync(OnResourceLoadFinished func)
    {
        ResourceTool.Instance.ResourceLoadState = ResourceLoadStateType.Loading;

        _callBack = func;
        if (_resource != null)
        {
            LoadFinished();
        }

        if (_resourceInfo.isFromAssetBundle)
        {
            ResourceTool.Instance.StartCoroutine(LoadAssetBundleAsync());
        }
        else
        {
            ResourceTool.Instance.StartCoroutine(LoadResourceAsync());
        }
    }

    IEnumerator LoadAssetBundleAsync()
    {
        AssetBundle bundle;
        AssetBundleRequest request;

        //载入assetbundle
        var assetbundlePath = string.Format("{0}{1}/{2}{3}", Application.streamingAssetsPath, ResourceTool.PREFIX_ASSETBUNDLE_PATH
                , _resourceInfo.assetbundlePath, ResourceTool.SUFFIX_ASSETBUNDLE_PATH);
        WWW www = new WWW(assetbundlePath);
        yield return www;
        bundle = www.assetBundle;

        //载入prefab
#if UNITY_5 || UNITY_2018
        request = bundle.LoadAssetAsync<GameObject>(_resourceInfo.resourceName);
#else
		request = bundle.LoadAsync(_resourceInfo.resourceName, typeof(GameObject));
#endif

        yield return request;
        _resource = request.asset;
        bundle.Unload(false);

        LoadFinished();
    }

    IEnumerator LoadResourceAsync()
    {
        var resourceRequest = Resources.LoadAsync<GameObject>(_resourceInfo.resourcePath);
        yield return resourceRequest;
        _resource = resourceRequest.asset;

        LoadFinished();
    }

    public void Load()
    {
        ResourceTool.Instance.ResourceLoadState = ResourceLoadStateType.Loading;

        _callBack = null;
        if (_resource != null)
        {
            LoadFinished();
        }

        if (_resourceInfo.isFromAssetBundle)
        {
            LoadFromAssetBundle();
        }
        else
        {
            LoadFromResource();
        }
    }

    void LoadFromAssetBundle()
    {
        var assetbundlePath = string.Format("{0}{1}/{2}{3}", Application.streamingAssetsPath, ResourceTool.PREFIX_ASSETBUNDLE_PATH
        , _resourceInfo.assetbundlePath, ResourceTool.SUFFIX_ASSETBUNDLE_PATH);
        byte[] assetBundleContent = null;
        FileUtil.LoadFileWithBytes(assetbundlePath, out assetBundleContent);

        var bundle = AssetBundle.LoadFromMemory(assetBundleContent);
#if UNITY_5 || UNITY_2018
        _resource = bundle.LoadAsset<GameObject>(_resourceInfo.resourceName);
#else
        _resource = bundle.Load<GameObject>(_resourceInfo.resourceName);
#endif
        bundle.Unload(true);

        LoadFinished();
    }

    public void LoadFromResource()
    {
        _resource = Resources.Load(_resourceInfo.resourcePath);

        LoadFinished();
    }

    public void LoadFinished()
    {
        if (_resource == null)
        {
            Debug.LogWarning(string.Format("Warning : Failed to load prefab {0}", _resourceInfo.resourceName));
            ResourceTool.Instance.ResourceLoadState = ResourceLoadStateType.Finished;
            return;
        }

        if (_callBack == null)
        {
            ResourceTool.Instance.ResourceLoadState = ResourceLoadStateType.Finished;
            return;
        }

        _callBack(_resource);
        _callBack = null;

        ResourceTool.Instance.ResourceLoadState = ResourceLoadStateType.Finished;
    }
}