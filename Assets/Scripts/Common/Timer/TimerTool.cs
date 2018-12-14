using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTool : ITimerTool
{
    List<TimerEvent> _timerEventList = new List<TimerEvent>();
    List<TimerEvent> _tmpTimerEventList = new List<TimerEvent>();

    public TimerEvent AddOnce(int delay, TimerCallback timerCallback)
    {
        return Add(delay, 1, 0, null, timerCallback);
    }

    public TimerEvent AddMulti(int delay, int count, int interval, TimerCallback timerCallback)
    {
        if (count <= 0)
        {
            LogUtil.E("AddMulti count should more than zero");
            return null;
        }

        return Add(delay, count, interval, null, timerCallback);
    }

    public TimerEvent AddEndLess(int delay, int interval, ITimerObject timer)
    {
        return Add(delay, -1, interval, timer);
    }

    TimerEvent Add(int delay, int count, int interval, ITimerObject timer, TimerCallback timerCallback = null)
    {
        if (timer == null && timerCallback == null) 
        {
            LogUtil.E("Timer and timerCallback are null!");
            return null;
        }

        var timerEvent = WorldManager.Instance.PoolMgr.Get<TimerEvent>();
        timerEvent.Init(delay, count, interval, timer, timerCallback);

        _tmpTimerEventList.Add(timerEvent);

        return timerEvent;
    }

    void Remove(TimerEvent timerEvent)
    {
        _timerEventList.Remove(timerEvent);
        WorldManager.Instance.PoolMgr.Release(timerEvent);
    }

    public void Update()
    {
        if (_tmpTimerEventList.Count == 0 && _timerEventList.Count == 0)
        {
            return;
        }

        if (_tmpTimerEventList.Count != 0)
        {
            _timerEventList.AddRange(_tmpTimerEventList);
            _tmpTimerEventList.Clear();
        }

        var gameSystemData = WorldManager.Instance.GameCore.GetData<Data.GameSystemData>();
        var currentTime = gameSystemData.unscaleTime;
        for (var i = 0; i < _timerEventList.Count;)
        {
            var timerEvent = _timerEventList[i];
            if (!timerEvent.IsFinished())
            {
                timerEvent.Invoke(currentTime);
                i++;
            }
            else
            {
                Remove(timerEvent);
            }
        }
    }

    public void Destroy()
    {
        _tmpTimerEventList.Clear();
        _timerEventList.Clear();
    }
}
