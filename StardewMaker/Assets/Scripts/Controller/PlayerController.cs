using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerInteraction
{
    None, // null
    Pick, // PickAxe
    Plant, // null?
    Water, // WateringCan
    Harvest, // null?
    Fish, // FishingRod
    GetWater, // WateringCan
    Axe
}

public class PlayerController : MonoBehaviour
{
    public static PlayerController Instance;

    private string TAG = "[PlayerController]";
    private UserInputManager inputManager;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }

    private Vector2 mouseWorldPos;
    private Vector2 moveInput, move;
    private Vector2 lastMove;
    private Vector2 playerToMouse;
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
        MouseLeftInput();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        curPos = rb.position;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        playerToMouse = (mouseWorldPos - curPos).normalized;
    }

    private void PlayerMoveInput()
    {
        moveInput = UserInputManager.Instance.inputActions.Player.Move.ReadValue<Vector2>();
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

    private void MouseLeftInput()
    {
        if (inputManager.inputActions.Player.MouseLeft.WasPressedThisFrame())
        {
            Collider2D mouseHit = Physics2D.OverlapPoint(mouseWorldPos);
            Collider2D[] playerHits = Physics2D.OverlapCircleAll(curPos, 1f);

            InteractWithObject(mouseHit, playerHits);
        }
    }

    private void InteractWithObject(Collider2D mouseHit, Collider2D[] playerHits)
    {
        ItemData currentItem = QuickSlotManager.Instance.slots[QuickSlotManager.Instance.currentSelectedIndex].itemData;

        foreach(Collider2D hit in playerHits)
        {
            if (hit == mouseHit)
            {
                Debug.Log($"hit Info : {hit.name}({hit.transform.position})");

                hit.TryGetComponent(out FarmLand farmLand);
                hit.TryGetComponent(out Tree tree);
                hit.TryGetComponent(out Pond pond);

                if (currentItem != null)
                {
                    switch (currentItem.itemType)
                    {
                        case ItemType.Seed:
                            if (farmLand != null)
                            {
                                var crop = CropManager.Instance.GetCropAt(farmLand.GetPosition());

                                if (crop == null && farmLand.landState != LandState.Normal)
                                {
                                    farmLand.Plant(currentItem);
                                    SetInteractAnimation(PlayerInteraction.Plant);
                                }
                            }
                            break;
                        case ItemType.Tool:
                            if (currentItem.name == "ToolHoe")
                            {
                                if (farmLand != null)
                                {
                                    if (farmLand.Pick())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Pick);
                                    }
                                }
                            }
                            else if (currentItem.name == "ToolWateringCan")
                            {
                                if (farmLand != null)
                                {
                                    if (farmLand.Water())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Water);
                                    }
                                }
                                else if (pond != null)
                                {
                                    pond.GetWater();
                                    SetInteractAnimation(PlayerInteraction.GetWater);
                                }
                            }
                            else if (currentItem.name == "ToolAxe")
                            {
                                if (tree != null)
                                {
                                    tree.Chop();
                                    SetInteractAnimation(PlayerInteraction.Axe);
                                }
                            }
                            else if (currentItem.name == "ToolFishingRod")
                            {
                                if (pond != null)
                                {
                                    pond.Fish();
                                    SetInteractAnimation(PlayerInteraction.Fish);
                                }
                            }
                            else if (currentItem.name == "ToolGlove")
                            {
                                if (farmLand != null)
                                {
                                    var crop = CropManager.Instance.GetCropAt(farmLand.GetPosition());

                                    if (crop != null && farmLand.Harvest())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Harvest);
                                    }
                                }
                            }
                            break;
                    }
                }
                else
                {

                }
            }
        }
    }

    private void SetInteractAnimation(PlayerInteraction interaction)
    {
        switch (interaction)
        {
            case PlayerInteraction.None:
                break;
            case PlayerInteraction.Pick:
                anim.SetBool("Pick", true);
                anim.SetFloat("MouseX", playerToMouse.x);
                anim.SetFloat("MouseY", playerToMouse.y);
                break;
            case PlayerInteraction.Plant:
                anim.SetBool("Plant", true);
                anim.SetFloat("MouseX", playerToMouse.x);
                anim.SetFloat("MouseY", playerToMouse.y);
                break;
            case PlayerInteraction.Water:
                anim.SetBool("Water", true);
                anim.SetFloat("MouseX", playerToMouse.x);
                anim.SetFloat("MouseY", playerToMouse.y);
                break;
            case PlayerInteraction.Harvest:
                anim.SetBool("Harvest", true);
                anim.SetFloat("MouseX", playerToMouse.x);
                anim.SetFloat("MouseY", playerToMouse.y);
                break;
            case PlayerInteraction.Fish:
                anim.SetBool("Fish", true);
                anim.SetFloat("MouseX", playerToMouse.x);
                anim.SetFloat("MouseY", playerToMouse.y);
                break;
            case PlayerInteraction.GetWater:
                anim.SetBool("GetWater", true);
                anim.SetFloat("MouseX", playerToMouse.x);
                anim.SetFloat("MouseY", playerToMouse.y);
                break;
            case PlayerInteraction.Axe:
                anim.SetBool("Axe", true);
                anim.SetFloat("MouseX", playerToMouse.x);
                anim.SetFloat("MouseY", playerToMouse.y);
                break;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(curPos, 1f);
    }

}
