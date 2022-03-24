using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwoompAnimation : MonoBehaviour
{
    public AnimationClip JumpOnTower(GameObject tower, int multiplier = 2)
    {
        Sprite sprite;
        AnimationClip clip = new AnimationClip();

        if (tower.GetComponent<WallToDefend>() != null)
        {
            sprite = tower.GetComponentInChildren<SpriteRenderer>()?.sprite;
        }
        else sprite = tower.transform.GetChild(4).GetComponent<SpriteRenderer>()?.sprite;

        if (sprite == null) return null;

        float spriteHeight = sprite.bounds.extents.y * multiplier;

        Keyframe[] keysX = new Keyframe[2];
        keysX[0] = new Keyframe(0f, gameObject.transform.position.x);
        keysX[1] = new Keyframe(1f, tower.transform.position.x);
        AnimationCurve curveX = new AnimationCurve(keysX);

        Keyframe[] keysY = new Keyframe[3];
        keysY[0] = new Keyframe(0f, gameObject.transform.position.y);
        keysY[1] = new Keyframe(0.5f, spriteHeight * 3);
        keysY[2] = new Keyframe(1f, spriteHeight);
        AnimationCurve curveY = new AnimationCurve(keysY);

        clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
        clip.name = "Jump";
        clip.legacy = true;

        return clip;
    }
}
