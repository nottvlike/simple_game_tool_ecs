using System.Net;
using System;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine.Networking;
using System.Collections;

public delegate void WebRequestResult(string content);

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

    public static void GetAsync(string url, WebRequestResult callback)
    {
        try
        {
            var webRequest = (HttpWebRequest)WebRequest.Create(url);

            var request = new RequestObject();
            request.webRequest = webRequest;
            request.webResult = callback;

            var result = webRequest.BeginGetResponse(new AsyncCallback(WebRequestCallback), request);
            ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(WebRequestTimeoutCallback), webRequest, WAIT_TIME, true);
        }
        catch(WebException e)
        {
            LogUtil.E("WebException raised, {0} {1}!", e.Status, e.Message);
        }
        catch(Exception e)
        {
            LogUtil.E("Exception raised, {0} {1}!", e.Source, e.Message);
        }
    }

    static void WebRequestCallback(IAsyncResult asynchronousResult)
    {
        var request = (RequestObject)asynchronousResult.AsyncState;
        var webRequest = request.webRequest;

        try
        {
            var response = (HttpWebResponse)webRequest.EndGetResponse(asynchronousResult);
            using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
            {
                var result = streamReader.ReadToEnd();

                var webResult = request.webResult;
                if (webResult != null)
                {
                    webResult(result);
                }
            }
        }
        catch (WebException e)
        {
            LogUtil.E("WebException raised, {0} {1}!", e.GetType(), e.Message);
        }
        catch (Exception e)
        {
            LogUtil.E("Exception raised, {0} {1}!", e.Source, e.Message);
        }
    }

    static void WebRequestTimeoutCallback(object state, bool timedOut)
    {
        if (timedOut)
        {
            HttpWebRequest request = state as HttpWebRequest;
            if (request != null)
            {
                request.Abort();
            }
        }
    }
}
