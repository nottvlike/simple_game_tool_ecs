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
    static int _logState = (int)LogState.Warning;

    public static void I(string format, params string[] args)
    {
        if (!CanLog(LogState.Info))
        {
            return;
        }

        Debug.LogFormat(format, args);
    }

    public static void W(string format, params string[] args)
    {
        if (!CanLog(LogState.Warning))
        {
            return;
        }

        Debug.LogWarningFormat(format, args);
    }

    public static void E(string format, params string[] args)
    {
        if (!CanLog(LogState.Error))
        {
            return;
        }

        Debug.LogErrorFormat(format, args);
    }

    static bool CanLog(LogState state)
    {
        return (int)state >= _logState;
    }
}
