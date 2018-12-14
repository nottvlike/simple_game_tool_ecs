using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine.Networking;
using System.Collections;

public enum WebRequestResultType
{
    Success = 0,
    ConnectFailure,
    TimeOut,
    Unknown
}

public delegate void WebRequestResult(WebRequestResultType result, string content);

public class HttpUtil
{
    struct RequestObject
    {
        public HttpWebRequest webRequest;
        public WebRequestResult webResult;
    }

    public static string Get(string url)
    {
        var result = "";
        try
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            var response = webRequest.GetResponse();
            using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                result = streamReader.ReadToEnd();
            }
        }
        catch (WebException e)
        {
            LogUtil.E("WebException raised, {0} {1}!", e.Status, e.Message);
        }
        catch (Exception e)
        {
            LogUtil.E("Exception raised, {0} {1}!", e.Source, e.Message);
        }

        return result;
    }

    const int WAIT_TIME = 4000;

    public static void GetAsync(string url, WebRequestResult resultCallback)
    {
        try
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            var request = new RequestObject();
            request.webRequest = webRequest;
            request.webResult = resultCallback;

            var result = webRequest.BeginGetResponse(new AsyncCallback(WebRequestCallback), request);
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(WebRequestTimeoutCallback), request, WAIT_TIME, true);
        }
        catch(WebException e)
        {
            LogUtil.E("WebException raised, {0} {1}!", e.Status, e.Message);
            WebRequestFinished(resultCallback, WebRequestResultType.ConnectFailure, string.Empty);
        }
        catch(Exception e)
        {
            LogUtil.E("Exception raised, {0} {1}!", e.Source, e.Message);
            WebRequestFinished(resultCallback, WebRequestResultType.Unknown, string.Empty);
        }
    }

    static void WebRequestCallback(IAsyncResult asynchronousResult)
    {
        var request = (RequestObject)asynchronousResult.AsyncState;
        var webRequest = request.webRequest;
        var webResult = request.webResult;

        try
        {
            var response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
            using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var result = streamReader.ReadToEnd();

                WebRequestFinished(webResult, WebRequestResultType.Success, result);
            }
        }
        catch (WebException e)
        {
            LogUtil.E("WebException raised, {0} {1}!", e.GetType(), e.Message);
            WebRequestFinished(webResult, WebRequestResultType.ConnectFailure, string.Empty);
        }
        catch (Exception e)
        {
            LogUtil.E("Exception raised, {0} {1}!", e.Source, e.Message);
            WebRequestFinished(webResult, WebRequestResultType.Unknown, string.Empty);
        }
    }

    static void WebRequestFinished(WebRequestResult callback, WebRequestResultType resultType, string result)
    {
        if (callback != null)
        {
            WorldManager.Instance.TimerMgr.AddOnce(0, delegate ()
            {
                callback(resultType, result);
            });
        }
    }

    static void WebRequestTimeoutCallback(object state, bool timedOut)
    {
        if (timedOut)
        {
            var request = (RequestObject)state;
            var webRequest = request.webRequest;
            if (webRequest != null)
            {
                webRequest.Abort();
            }

            var resultCallback = request.webResult;
            if (resultCallback != null)
            {
                resultCallback(WebRequestResultType.TimeOut, string.Empty);
            }
        }
    }
}
