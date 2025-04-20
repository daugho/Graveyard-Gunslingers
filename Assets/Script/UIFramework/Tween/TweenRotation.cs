using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using uTools;

public class TweenRotation : uTween<Vector3>
{

    Transform trans;

    public Transform cacheRectTransfrom
    {
        get
        {
            if (trans == null)
            {
                trans = GetComponent<Transform>();
            }

            return trans;
        }
    }

    public Quaternion QuaternionValue
    {
        get
        {
            return cacheRectTransfrom.localRotation;
        }
        set
        {
            cacheRectTransfrom.localRotation = value;
        }
    }

    protected override void OnUpdate(float _factor, bool _isFinished)
    {
        QuaternionValue = Quaternion.Euler(Vector3.Lerp(from, to, _factor));
    }

    public static TweenRotation Begin(GameObject go, Vector3 from, Vector3 to, float duration = 1f, float delay = 0f)
    {
        TweenRotation comp = Begin<TweenRotation>(go, duration);
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
