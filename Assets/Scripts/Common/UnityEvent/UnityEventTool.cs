using System;
using System.Collections.Generic;

public class UnityEventTool : MonoSingleton<UnityEventTool>, IUnityEventTool
{
	List<IUpdateEvent> _updateEventList = new List<IUpdateEvent>();

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
}