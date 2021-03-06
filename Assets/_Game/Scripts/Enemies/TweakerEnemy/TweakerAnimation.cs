using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TweakerAnimation : MonoBehaviour
{
    private int _called = 0;

    public AnimationClip HopAnimation()
    {
        AnimationClip clip = new AnimationClip();

        float xStartPos = gameObject.transform.position.x;
        float yStartPos = gameObject.transform.position.y;

        Keyframe[] keysX = new Keyframe[2];
        keysX[0] = new Keyframe(0f, xStartPos);
        keysX[1] = new Keyframe(1f, xStartPos - 5f);
        AnimationCurve curveX = new AnimationCurve(keysX);

        Keyframe[] keysY = new Keyframe[3];
        keysY[0] = new Keyframe(0f, gameObject.transform.position.y);
        keysY[1] = new Keyframe(0.5f, gameObject.transform.position.y + 2f);
        keysY[2] = new Keyframe(1f, gameObject.transform.position.y);
        AnimationCurve curveY = new AnimationCurve(keysY);

        clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
        clip.name = "Hop" + _called;
        clip.legacy = true;

        _called++;
        return clip;
    }
}
