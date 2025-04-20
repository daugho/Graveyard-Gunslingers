using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uTools;

public class TweenScale : uTween<Vector3>
{

    Transform mRectTransform;

    public Transform cachedRectTransform { get { if (mRectTransform == null) mRectTransform = GetComponent<Transform>(); return mRectTransform; } }
    public override Vector3 value
    {
        get { return cachedRectTransform.localScale; }
        set { cachedRectTransform.localScale = value; }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from + factor * (to - from);
    }

    public static TweenScale Begin(GameObject go, Vector3 from, Vector3 to, float duration = 1f, float delay = 0f)
    {
        TweenScale comp = Begin<TweenScale>(go, duration);
        comp.value = from;
        comp.from = from;
        comp.to = to;
        comp.duration = duration;
        comp.delay = delay;
        if (duration <= 0)
        {
            comp.Sample(1, true);
            comp.enabled = false;
        }
        return comp;
    }
}
