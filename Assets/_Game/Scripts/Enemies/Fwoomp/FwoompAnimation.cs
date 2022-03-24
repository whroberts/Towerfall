using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FwoompAnimation : MonoBehaviour
{
    private FwoompEnemy _base;

    private void Awake()
    {
        _base = GetComponent<FwoompEnemy>();
    }

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
        keysY[1] = new Keyframe(0.5f, spriteHeight * multiplier);
        keysY[2] = new Keyframe(1f, spriteHeight);
        AnimationCurve curveY = new AnimationCurve(keysY);

        clip.SetCurve("", typeof(Transform), "localPosition.x", curveX);
        clip.SetCurve("", typeof(Transform), "localPosition.y", curveY);
        clip.name = "Jump";
        clip.legacy = true;

        return clip;
    }

    private float _distance = 0;
    private float _startTime = 0;
    private bool _startLerp = false;

    public void SetLerp(GameObject tower, float startTime, int multiplier = 2)
    {
        Sprite sprite;

        if (tower.GetComponent<WallToDefend>() != null)
        {
            sprite = tower.GetComponentInChildren<SpriteRenderer>()?.sprite;
        }
        else sprite = tower.transform.GetChild(4).GetComponent<SpriteRenderer>()?.sprite;

        if (sprite != null)
        {
            float spriteHeight = sprite.bounds.extents.y * multiplier;

            Vector3 startPos = gameObject.transform.position;
            Vector3 midPos = new Vector3(tower.transform.position.x + gameObject.transform.position.x, spriteHeight * multiplier, 0f);
            Vector3 endPos = new Vector3(tower.transform.position.x, spriteHeight * 1.5f, 0f);

            StartCoroutine(LerpToTower(startPos, midPos, endPos, startTime));
        }
    }



    private void Update()
    {
        _distance = (Time.time - _startTime) * 1;

    }

    private IEnumerator LerpToTower(Vector3 startPos, Vector3 midPos, Vector3 endPos, float startTime)
    {
        float t = 0;
        var rb = GetComponent<Rigidbody2D>();
        float gravity = 0;
        gravity = rb.gravityScale;
        rb.gravityScale = 0;

        Collider2D col = GetComponent<Collider2D>();
        col.enabled = false;

        while (t < 1)
        {
            yield return null;
            t += (Time.deltaTime / startTime) * 10f;
            transform.position = Vector3.Lerp(startPos, endPos, t);
        }

        col.enabled = true;
        _base._attackTrigger.enabled = true;
        rb.gravityScale = gravity;
        transform.position = endPos;
    }
}
