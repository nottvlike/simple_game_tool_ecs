using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerTool : ITimerTool
{
    List<TimerEvent> _timerEventList = new List<TimerEvent>();
    List<TimerEvent> _tmpTimerEventList = new List<TimerEvent>();

    TimerEvent Add(int delay, int count, int interval, ITimerObject timer)
    {
        if (timer == null)
        {
            LogUtil.E("Timer object should not be null!");
            return null;
        }

        var timerEvent = WorldManager.Instance.PoolMgr.Get<TimerEvent>();
        timerEvent.Init(delay, count, interval, timer);

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

    public TimerEvent AddEndLess(int delay, int interval, ITimerObject timer)
    {
        return Add(delay, -1, interval, timer);
    }

    public TimerEvent AddMulti(int delay, int count, int interval, ITimerObject timer)
    {
        if (count <= 0)
        {
            LogUtil.E("AddMulti count should more than zero");
            return null;
        }

        return Add(delay, count, interval, timer);
    }

    public TimerEvent AddOnce(int delay, ITimerObject timer)
    {
        return Add(delay, 1, 0, timer);
    }

    public void Destroy()
    {
        _tmpTimerEventList.Clear();
        _timerEventList.Clear();
    }
}
