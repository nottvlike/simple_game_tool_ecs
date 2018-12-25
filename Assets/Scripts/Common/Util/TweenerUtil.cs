using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TweenerUtil
{
    public static void Move(GameObject target, int delay, int duration, Vector3 start, Vector3 end, UnityAction callback = null,
        bool isLoop = false, int loopCount = -1, LoopType loopType = LoopType.Loop)
    {
        var move = WorldManager.Instance.PoolMgr.Get<Move>();
        move.enabled = true;
        move.target = target;
        move.startDelay = delay;
        move.duration = duration;
        move.animationCurve.AddKey(0, 0);
        move.animationCurve.AddKey(1, (float)duration / Constant.SECOND_TO_MILLISECOND);
        move.startPosition = start;
        move.targetPosition = end;
        move.isLoop = isLoop;
        move.loops = loopCount;
        move.loopType = loopType;

        if (move.onFinished == null)
        {
            move.onFinished = new UnityEvent();
        }

        if (callback != null)
        {
            move.onFinished.AddListener(callback);
        }
        move.Play();
    }
}
