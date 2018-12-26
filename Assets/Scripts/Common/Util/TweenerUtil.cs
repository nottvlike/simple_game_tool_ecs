using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TweenerUtil
{
    public static Tweener Move(GameObject target, int delay, int duration, Vector3 start, Vector3 end, UnityAction<Tweener> callback = null,
        bool isLoop = false, int loopCount = -1, LoopType loopType = LoopType.Loop)
    {
        var move = WorldManager.Instance.PoolMgr.Get<Move>();
        move.startPosition = start;
        move.targetPosition = end;

        AddTweener(move, target, delay, duration, callback, isLoop, loopCount, loopType);
        return move;
    }

    public static Tweener Rotate(GameObject target, int delay, int duration, Vector3 from, Vector3 to, UnityAction<Tweener> callback = null,
        bool isLoop = false, int loopCount = -1, LoopType loopType = LoopType.Loop)
    {
        var rotate = WorldManager.Instance.PoolMgr.Get<Rotate>();
        rotate.from = from;
        rotate.to = to;

        AddTweener(rotate, target, delay, duration, callback, isLoop, loopCount, loopType);
        return rotate;
    }

    public static Tweener Scale(GameObject target, int delay, int duration, Vector3 from, Vector3 to, UnityAction<Tweener> callback = null,
        bool isLoop = false, int loopCount = -1, LoopType loopType = LoopType.Loop)
    {
        var scale = WorldManager.Instance.PoolMgr.Get<Scale>();
        scale.from = from;
        scale.to = to;

        AddTweener(scale, target, delay, duration, callback, isLoop, loopCount, loopType);
        return scale;
    }

    public static Tweener Fade(GameObject target, int delay, int duration, float from, float to, UnityAction<Tweener> callback = null,
        bool isLoop = false, int loopCount = -1, LoopType loopType = LoopType.Loop)
    {
        var fade = WorldManager.Instance.PoolMgr.Get<Fade>();
        fade.from = from;
        fade.to = to;

        AddTweener(fade, target, delay, duration, callback, isLoop, loopCount, loopType);
        return fade;
    }

    static void AddTweener(Tweener tweener, GameObject target, int delay, int duration, UnityAction<Tweener> callback,
        bool isLoop, int loopCount, LoopType loopType)
    {
        tweener.enabled = true;
        tweener.target = target;
        tweener.startDelay = delay;
        tweener.duration = duration;
        tweener.animationCurve.AddKey(0, 0);
        tweener.animationCurve.AddKey((float)duration / Constant.SECOND_TO_MILLISECOND, 1);
        tweener.isLoop = isLoop;
        tweener.loops = loopCount;
        tweener.loopType = loopType;

        if (tweener.onFinished == null)
        {
            tweener.onFinished = new TweenerFinished();
        }

        if (callback != null)
        {
            tweener.onFinished.AddListener(callback);
        }

        tweener.onFinished.AddListener(Release);

        tweener.Play();
    }

    static void Release(Tweener tweener)
    {
        WorldManager.Instance.PoolMgr.Release(tweener);
    }
}
