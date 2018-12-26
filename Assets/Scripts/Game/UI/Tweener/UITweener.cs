using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public enum AnimationType
{
    None,
    Open,
    Close
}

public class UITweener : MonoBehaviour
{
    [System.Serializable]
    public class Animation
    {
        public AnimationType animationType;
        public bool autoStart;

        public Move moveTweener;
        public Rotate rotateTweener;
        public Scale scaleTweener;
        public Fade fadeTweener;

        public UnityEvent onFinished;

        List<Tweener> _tweenerList = new List<Tweener>();

        public void Play(bool isForce = false)
        {
            ColloctTweeners();

            var maxIndex = GetMaxTotalDurationIndex();
            for (var i = 0; i < _tweenerList.Count; i++)
            {
                var tweener = _tweenerList[i];
                if (tweener.enabled)
                {
                    if (i == maxIndex)
                    {
                        tweener.onFinished.AddListener(OnFinished);
                    }

                    if (!tweener.IsPlaying)
                    {
                        tweener.Play();
                    }
                    else if (tweener.IsPlaying && isForce)
                    {
                        tweener.ForcePlay();
                    }
                }
            }
        }

        public void Stop()
        {
            ColloctTweeners();

            for (var i = 0; i < _tweenerList.Count; i++)
            {
                var tweener = _tweenerList[i];
                if (tweener.enabled)
                {
                    tweener.Stop();
                }
            }
        }

        public void ColloctTweeners()
        {
            _tweenerList.Clear();

            _tweenerList.Add(moveTweener);
            _tweenerList.Add(rotateTweener);
            _tweenerList.Add(scaleTweener);
            _tweenerList.Add(fadeTweener);
        }

        void OnFinished(Tweener tweener)
        {
            if (onFinished != null)
            {
                onFinished.Invoke();
            } 
        }

        int GetMaxTotalDurationIndex()
        {
            var index = 0;
            var max = float.MinValue;
            for (var i = 0; i < _tweenerList.Count; i++)
            {
                var tweener = _tweenerList[i];
                if (max < tweener.TotalDuration)
                {
                    max = tweener.TotalDuration;
                    index = i;
                }
            }

            return index;
        }
    }

    public List<Animation> animationList = new List<Animation>();

    public void Play(AnimationType animationType, bool isForce = false)
    {
        for (var i = 0; i < animationList.Count; i++)
        {
            var tweener = animationList[i];
            if (tweener.animationType == animationType)
            {
                tweener.Play(isForce);
            }
        }
    }

    public void Stop(AnimationType animationType)
    {
        for (var i = 0; i < animationList.Count; i++)
        {
            var tweener = animationList[i];
            if (tweener.animationType == animationType)
            {
                tweener.Stop();
            }
        }
    }

    void OnEnable()
    {
        for (var i = 0; i < animationList.Count; i++)
        {
            var tweener = animationList[i];
            if (tweener.autoStart)
            {
                tweener.Play();
            }
        }
    }

    void OnDisable()
    {
        for (var i = 0; i < animationList.Count; i++)
        {
            var tweener = animationList[i];
            if (tweener.autoStart)
            {
                tweener.Stop();
            }
        }
    }
}
