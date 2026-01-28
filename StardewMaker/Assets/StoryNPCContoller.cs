using UnityEngine;

public class StoryNPCContoller : MonoBehaviour
{
    public enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }

    Animator anim;

    public void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void Move(Direction dir, int amount)
    {

        
    }
}
