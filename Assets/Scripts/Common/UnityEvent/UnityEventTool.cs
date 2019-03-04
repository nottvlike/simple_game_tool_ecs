using System;
using System.Collections.Generic;

public class UnityEventTool : MonoSingleton<UnityEventTool>, IUnityEventTool
{
	List<IUpdateEvent> _updateEventList = new List<IUpdateEvent>();
    List<IFixedUpdateEvent> _fixedUpdateEventList = new List<IFixedUpdateEvent>();
    List<ILateUpdateEvent> _lateUpdateEventList = new List<ILateUpdateEvent>();

    void Awake()
    {
        WorldManager.Instance.LaunchGame();
    }

    public void Destroy()
    {
    }

    void OnApplicationQuit()
    {
        WorldManager.Instance.Destroy();
    }

    void Update()
    {
        for (var i = 0; i < _updateEventList.Count; i++)
        {
            _updateEventList[i].Update();
        }
    }

    void FixedUpdate()
    {
        for (var i = 0; i < _fixedUpdateEventList.Count; i++)
        {
            _fixedUpdateEventList[i].FixedUpdate();
        }
    }

    void LateUpdate()
    {
        for (var i = 0; i < _lateUpdateEventList.Count; i++)
        {
            _lateUpdateEventList[i].LateUpdate();
        }
    }

    public bool IsAdded(IUpdateEvent updateEvent)
    {
        return _updateEventList.IndexOf(updateEvent) != -1;
    }

    public void Add(IUpdateEvent updateEvent)
	{
        if (IsAdded(updateEvent))
        {
            LogUtil.W("UpdateEvent has been added!");
            return;
        }

        _updateEventList.Add(updateEvent);
    }

	public void Remove(IUpdateEvent updateEvent)
	{
        if (!IsAdded(updateEvent))
        {
            LogUtil.W("UpdateEvent remove failed, not found!");
            return;
        }

        _updateEventList.Remove(updateEvent);
    }

    public bool IsAdded(IFixedUpdateEvent updateEvent)
    {
        return _fixedUpdateEventList.IndexOf(updateEvent) != -1;
    }

    public void Add(IFixedUpdateEvent updateEvent)
    {
        if (IsAdded(updateEvent))
        {
            LogUtil.W("UpdateEvent has been added!");
            return;
        }

        _fixedUpdateEventList.Add(updateEvent);
    }

    public void Remove(IFixedUpdateEvent updateEvent)
    {
        if (!IsAdded(updateEvent))
        {
            LogUtil.W("UpdateEvent remove failed, not found!");
            return;
        }

        _fixedUpdateEventList.Remove(updateEvent);
    }

    public bool IsAdded(ILateUpdateEvent updateEvent)
    {
        return _lateUpdateEventList.IndexOf(updateEvent) != -1;
    }

    public void Add(ILateUpdateEvent updateEvent)
    {
        if (IsAdded(updateEvent))
        {
            LogUtil.W("LateUpdateEvent has been added!");
            return;
        }

        _lateUpdateEventList.Add(updateEvent);
    }

    public void Remove(ILateUpdateEvent updateEvent)
    {
        if (!IsAdded(updateEvent))
        {
            LogUtil.W("LateUpdateEvent remove failed, not found!");
            return;
        }

        _lateUpdateEventList.Remove(updateEvent);
    }
}