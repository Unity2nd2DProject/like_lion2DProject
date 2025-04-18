using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

enum PlayerInteraction
{
    None,
    Pick,
    Plant,
    Water,
    Harvest,
    Fish,
    GetWater
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private string TAG = "[PlayerController]";
    private UserInputManager inputManager;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    private Vector2 moveInput, move;
    public float moveSpeed = 5f;
    private Vector2 curPos;

    public LayerMask whatIsLand;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        anim = GetComponentInChildren<Animator>();
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
        IInput();
        NInput();
        OneInput();
        TwoInput();
        ThreeInput();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        curPos = rb.position;
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

    private void XInput()
    {
        if (inputManager.inputActions.Player.X.WasPressedThisFrame())
        {
            CropManager.Instance.NextDay();
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

    private void IInput()
    {
        if (inputManager.inputActions.Player.I.WasPressedThisFrame())
        {
            InventoryUI.Instance.ToggleInventory();
        }
    }

    private void NInput() // Text NextDay
    {
        if (inputManager.inputActions.Player.N.WasPressedThisFrame())
        {
            GameManager.Instance.NextDay();
        }
    }

    private void OneInput() // Pick
    {
        if (inputManager.inputActions.Player._1.WasPressedThisFrame())
        {
            InteractWithLand(PlayerInteraction.Pick);
        }
    }

    private void TwoInput() // Water
    {
        if (inputManager.inputActions.Player._2.WasPressedThisFrame())
        {
            InteractWithLand(PlayerInteraction.Water);
        }
    }

    private void ThreeInput() // Harvest
    {
        if (inputManager.inputActions.Player._3.WasPressedThisFrame())
        {
            InteractWithLand(PlayerInteraction.Harvest);
        }
    }

    public bool Plant(CropData cropData)
    {
        return (InteractWithLand(PlayerInteraction.Plant, cropData));
    }

    private bool InteractWithLand(PlayerInteraction interaction, CropData cropData = null)
    {
        //Vector2Int landPos = Vector2Int.RoundToInt(curPos);
        Collider2D hit = Physics2D.OverlapCircle(curPos, 0.1f, whatIsLand);

        if (hit != null)
        {
            FarmLand land = hit.GetComponent<FarmLand>();
            if (land != null)
            {
                if (interaction == PlayerInteraction.Pick)
                {
                    return (land.Pick());
                }
                else if (interaction == PlayerInteraction.Water)
                {
                    return (land.Water());
                }
                else if (interaction == PlayerInteraction.Plant)
                {
                    return (land.Plant(cropData));
                }
                else if (interaction == PlayerInteraction.Harvest)
                {
                    return (land.Harvest());
                }
            }
        }

        return false;
    }

}
