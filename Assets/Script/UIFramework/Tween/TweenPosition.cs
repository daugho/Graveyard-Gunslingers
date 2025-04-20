using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uTools;

public class TweenPosition : uTween<Vector3>
{

    Transform trans;

    public Transform cachedRectTransform { get { if (trans == null) trans = GetComponent<Transform>(); return trans; } }
    public override Vector3 value
    {
        get { return cachedRectTransform.localPosition; }
        set { cachedRectTransform.localPosition = value; }
    }

    protected override void OnUpdate(float factor, bool isFinished)
    {
        value = from + factor * (to - from);
    }

    public static TweenPosition Begin(GameObject go, Vector3 from, Vector3 to, float duration = 1f, float delay = 0f)
    {
        TweenPosition comp = Begin<TweenPosition>(go, duration);
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
