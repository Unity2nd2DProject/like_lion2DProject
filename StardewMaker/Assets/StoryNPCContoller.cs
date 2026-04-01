using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StoryNPCContoller : MonoBehaviour
{    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    [HideInInspector]
    public StoryID currentStoryID;

    Animator anim;

    [Header("Settings")]
    public float tileSize = 1f;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Move(Direction dir, float time, float amount)
    {
        StartCoroutine(MoveCoroutine(dir, time));
    }

    private IEnumerator MoveCoroutine(Direction dir, float time)
    {

        Vector3 start = transform.position;
        Vector3 target = start + GetDirectionVector(dir) * tileSize;

        SetAnimation(dir, true);

        float elapsed = 0f;
        while (elapsed < time)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / time);
            transform.position = Vector3.Lerp(start, target, t);
            yield return null;
        }

        transform.position = target;

        SetAnimation(dir, false);
    }

    private Vector3 GetDirectionVector(Direction dir)
    {
        return dir switch
        {
            Direction.Up => Vector3.up,
            Direction.Down => Vector3.down,
            Direction.Left => Vector3.left,
            Direction.Right => Vector3.right,
            _ => Vector3.zero
        };
    }

    private void SetAnimation(Direction dir, bool isMoving)
    {
        if (anim == null) return;

        switch (dir)
        {
            case Direction.Up: anim.SetFloat("MoveX", 0); anim.SetFloat("MoveY", 1); break;
            case Direction.Down: anim.SetFloat("MoveX", 0); anim.SetFloat("MoveY", -1); break;
            case Direction.Left: anim.SetFloat("MoveX", -1); anim.SetFloat("MoveY", 0); break;
            case Direction.Right: anim.SetFloat("MoveX", 1); anim.SetFloat("MoveY", 0); break;
        }

        anim.SetBool("IsMoving", isMoving);
    }
}
