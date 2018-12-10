using UnityEngine;
using System.Collections;

public enum LogState
{
    Info = 0,
    Warning = 1,
    Error = 2
}

public class LogUtil
{
    static int _logState = (int)LogState.Info;

    public static void I(string format, params object[] args)
    {
        if (!CanLog(LogState.Info))
        {
            return;
        }

        if (args != null && args.Length != 0)
        {
            Debug.LogFormat(format, args);
        }
        else
        {
            Debug.Log(format);
        }
    }

    public static void W(string format, params object[] args)
    {
        if (!CanLog(LogState.Warning))
        {
            return;
        }


        if (args != null && args.Length != 0)
        {
            Debug.LogWarningFormat(format, args);
        }
        else
        {
            Debug.LogWarning(format);
        }
    }

    public static void E(string format, params object[] args)
    {
        if (!CanLog(LogState.Error))
        {
            return;
        }


        if (args != null && args.Length != 0)
        {
            Debug.LogErrorFormat(format, args);
        }
        else
        {
            Debug.LogError(format);
        }
    }

    static bool CanLog(LogState state)
    {
        return (int)state >= _logState;
    }
}
