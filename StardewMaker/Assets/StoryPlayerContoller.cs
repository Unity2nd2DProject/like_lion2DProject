using UnityEngine;

public class StoryPlayerContoller : MonoBehaviour
{

    [Header("Move")]
    private Vector2 moveInput;

    private Vector2 move;
    private Vector2 lastMove;

    private Vector2 playerToMouse;
    public float moveSpeed = 5f;
    private Vector2 curPos;

    private bool canMove = true;

    Animator anim;
    Rigidbody2D rb;

    private void Awake()
    {

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        MoveInput();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        curPos = rb.position;
    }

    private void MoveInput()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveInput = new Vector2(h, v).normalized;
        move = moveInput;

        if (move != Vector2.zero)
        {
            lastMove = move;
        }

        SetMoveAnimation();
    }

    private void SetMoveAnimation()
    {
        anim.SetBool("Move", move != Vector2.zero);
        anim.SetFloat("InputX", move.x);
        anim.SetFloat("InputY", move.y);
        anim.SetFloat("LastX", lastMove.x);
        anim.SetFloat("LastY", lastMove.y);
    }
}

