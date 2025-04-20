using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

enum PlayerInteraction
{
    None, // null
    Pick, // PickAxe
    Plant, // null?
    Water, // WateringCan
    Harvest, // null?
    Fish, // FishingRod
    GetWater // WateringCan
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
        MouseLeftInput();
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

    private void NInput() // NextDay (Test)
    {
        if (inputManager.inputActions.Player.N.WasPressedThisFrame())
        {
            GameManager.Instance.NextDay();
        }
    }

    private void OneInput() // Change CurrentTool (Test)
    {
        if (inputManager.inputActions.Player._1.WasPressedThisFrame())
        {
            CurrentToolManager.Instance.NextTool();
            //Debug.Log($"Current Tool : {CurrentToolManager.Instance.currentTool.name}");
        }
    }


    private void MouseLeftInput()
    {
        if (inputManager.inputActions.Player.MouseLeft.WasPressedThisFrame())
        {
            ItemData currentTool = CurrentToolManager.Instance.currentTool;

            Vector2 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Collider2D mouseHit = Physics2D.OverlapPoint(mouseWorldPos, whatIsLand);
            Collider2D[] playerHits = Physics2D.OverlapBoxAll(curPos, new Vector2(3f, 3f), 0f, whatIsLand);

            foreach (Collider2D hit in playerHits)
            {
                if (hit == mouseHit)
                {
                    FarmLand land = hit.GetComponent<FarmLand>();
                    if (land != null)
                    {
                        if (currentTool == null)
                        {
                            var crop = CropManager.Instance.GetCropAt(land.GetPosition());

                            if (crop == null)
                            {
                                if (land.landState == LandState.Fertile)
                                {
                                    //land.Plant(cropData);
                                }
                            }
                            else
                            {
                                if (crop.IsHarvestable())
                                {
                                    land.Harvest();
                                }
                            }
                        }
                        else if (currentTool.name == "ToolHoe")
                        {
                            land.Pick();
                        }
                        else if (currentTool.name == "ToolWateringCan")
                        {
                            land.Water();
                        }
                        else if (currentTool.name == "ToolAxe")
                        {

                        }
                        else if (currentTool.name == "ToolFishingRod")
                        {

                        }
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(curPos, new Vector2(3f, 3f));
    }

}
