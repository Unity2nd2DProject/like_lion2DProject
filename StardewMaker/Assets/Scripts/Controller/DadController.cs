using System.Collections;
using UnityEngine;

public class DadController : MonoBehaviour
{
    private string TAG = "[DadController]";
    private UserInputManager inputManager;

    private Rigidbody2D rb;

    private Vector2 moveInput, move;
    public float moveSpeed = 5f; // 이동 속도

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance;
    }

    void Update()
    {
        PlayerMoveInput();
        SpaceInput();
        ESCInput();
        ZInput();
        XInput();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
    }

    private void PlayerMoveInput()
    {
        moveInput = UserInputManager.Instance.inputActions.Player.Move.ReadValue<Vector2>();
        move = moveInput;
    }

    private void SpaceInput()
    {
        if (inputManager.inputActions.Player.Space.WasPressedThisFrame())
        {

        }
    }

    private void ESCInput()
    {
        if (inputManager.inputActions.Player.ESC.WasPressedThisFrame())
        {
            Debug.Log($"{TAG} ESCInput IsPressed. UI 인풋으로 전환");
            GameManager.Instance.SetGameState(TAG, GameState.UI);
        }
    }

    private void ZInput()
    {
        if (inputManager.inputActions.Player.Z.WasPressedThisFrame())
        {
            GameManager.Instance.changeScene("Connect1"); // 테스트용
        }
    }

    void XInput()
    {
        if (inputManager.inputActions.Player.X.WasPressedThisFrame())
        {

        }
        if (inputManager.inputActions.Player.X.IsPressed())
        {
            // Debug.Log($"{TAG} EnterInput IsPressed");
        }
        if (inputManager.inputActions.Player.X.WasReleasedThisFrame())
        {
            // Debug.Log($"{TAG} EnterInput WasReleasedThisFrame");
        }
    }
}
