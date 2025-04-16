using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }

    //private void OnMove(InputValue value)
    //{
    //    movementInput = value.Get<Vector2>();
    //}
}
