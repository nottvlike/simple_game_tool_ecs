using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class WorldManager : Singleton<WorldManager>
{
    ITimerTool _timerTool;
    public ITimerTool TimerMgr
    {
        get
        {
            if (_timerTool == null)
                _timerTool = new TimerTool();

            return _timerTool;
        }
    }

    void DestroyTimerMgr()
    {
        if (_timerTool != null)
        {
            _timerTool.Destroy();
            _timerTool = null;
        }
    }
}
