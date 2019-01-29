using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Object/Animation Curve", order = int.MaxValue)]
public class AnimationCurveData : ScriptableObject
{
    public AnimationCurve curve;

    public float Evaluate(float time)
    {
        return curve.Evaluate(time);
    }
}
