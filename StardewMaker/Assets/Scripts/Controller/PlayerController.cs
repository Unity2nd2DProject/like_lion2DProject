using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum PlayerInteraction
{
    None,
    Pick, 
    Plant,
    Water, 
    Harvest, 
    Fish, 
    GetWater,
    Axe,
    Fertilize
}

public class PlayerController : Singleton<PlayerController>
{
    //public static PlayerController Instance;

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
    private bool canMove = true;

    private FarmLand curFarmLand;
    private Pond curPond;
    private Tree curTree;
    private ItemData curItem;

    public bool justTeleported = false;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    //void Awake()
    //{
    //    if (Instance == null)
    //    {
    //        Instance = this;
    //    }

    //    anim = GetComponentInChildren<Animator>();
    //    rb = GetComponent<Rigidbody2D>();
    //}

    private void OnEnable()
    {
        inputManager = UserInputManager.Instance;
    }

    void Update()
    {
        if (!canMove)
        {
            return;
        }

        PlayerMoveInput();
        SpaceInput();
        ESCInput();
        ZInput();
        XInput();
        IInput();
        F1Input();
        NInput();
        MouseLeftInput();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        curPos = rb.position;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //playerToMouse = (mouseWorldPos - curPos).normalized;

        Vector2 direction = mouseWorldPos - curPos;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            playerToMouse = direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            playerToMouse = direction.y > 0 ? Vector2.up : Vector2.down;
        }
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
            //GameManager.Instance.changeScene("Connect1"); // 테스트용
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
            UIManager.Instance.inventoryUI.ToggleInventory();
        }
    }

    private void NInput() // NextDay (Test)
    {
        if (inputManager.inputActions.Player.N.WasPressedThisFrame())
        {
            TimeManager.Instance.AdvanceDay();
        }
    }

    private void F1Input() // Save
    {
        if (inputManager.inputActions.Player.F1.WasPressedThisFrame())
        {
            SaveManager.Instance.Save();
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
        curItem = InventoryManager.Instance.GetQuickSlotCurrentSelectedItem();

        foreach (Collider2D hit in playerHits)
        {
            if (hit == mouseHit)
            {
                SetTarget(hit);

                if (curItem != null)
                {
                    switch (curItem.itemType)
                    {
                        case ItemType.Seed:
                            if (curFarmLand != null)
                            {
                                //var crop = CropManager.Instance.GetCropAt(curFarmLand.GetPosition());

                                if (curFarmLand.CanPlant(curItem))
                                {
                                    SetInteractAnimation(PlayerInteraction.Plant);
                                }
                            }
                            break;
                        case ItemType.Etc:
                            if (curItem.name == "Fertilizer" && curFarmLand != null)
                            {
                                if (curFarmLand.CanFertilze())
                                {
                                    SetInteractAnimation(PlayerInteraction.Fertilize);
                                }
                            }
                            break;
                        case ItemType.Tool:
                            if (curItem.name == "ToolHoe")
                            {
                                if (curFarmLand != null)
                                {
                                    if (curFarmLand.CanPick())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Pick);
                                    }
                                }
                            }
                            else if (curItem.name == "ToolWateringCan")
                            {
                                if (curFarmLand != null)
                                {
                                    if (curFarmLand.CanWater())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Water);
                                    }
                                }
                                else if (curPond != null)
                                {
                                    SetInteractAnimation(PlayerInteraction.GetWater);
                                }
                            }
                            else if (curItem.name == "ToolAxe")
                            {
                                if (curTree != null)
                                {
                                    SetInteractAnimation(PlayerInteraction.Axe);
                                }
                            }
                            else if (curItem.name == "ToolFishingRod")
                            {
                                if (curPond != null)
                                {
                                    SetInteractAnimation(PlayerInteraction.Fish);
                                }
                            }
                            else if (curItem.name == "ToolGlove")
                            {
                                if (curFarmLand != null)
                                {
                                    var crop = CropManager.Instance.GetCropAt(curFarmLand.GetPosition());

                                    if (crop != null && curFarmLand.CanHarvest())
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

    private void SetTarget(Collider2D hit)
    {
        curFarmLand = null;
        curPond = null;
        curTree = null;

        if (hit.TryGetComponent(out FarmLand farmLand))
        {
            curFarmLand = farmLand;
        }
        else if (hit.TryGetComponent(out Tree tree))
        {
            curTree = tree;
        }
        else if (hit.TryGetComponent(out Pond pond))
        {
            curPond = pond;
        }

        Debug.Log($"{curFarmLand} {curTree} {curPond}");
    }

    public void Harvest()
    {
        curFarmLand.Harvest();
    }

    public void Fish()
    {
        curPond.Fish();
    }

    public void Chop()
    {
        curTree.Chop();
    }

    public void GetWater()
    {
        curPond.GetWater();
    }

    public void Water()
    {
        curFarmLand.Water();
    }

    public void Pick()
    {
        curFarmLand.Pick();
    }

    public void Plant()
    {
        curFarmLand.Plant(curItem);
    }

    public void Fertlize()
    {
        curFarmLand.Fertilize();
    }

    private void SetInteractAnimation(PlayerInteraction interaction)
    {
        switch (interaction)
        {
            case PlayerInteraction.Pick:
                anim.SetBool("Pick", true);
                break;
            case PlayerInteraction.Plant:
                anim.SetBool("Plant", true);
                break;
            case PlayerInteraction.Water:
                anim.SetBool("Water", true);
                break;
            case PlayerInteraction.Harvest:
                anim.SetBool("Harvest", true);
                break;
            case PlayerInteraction.Fish:
                anim.SetBool("Fish", true);
                break;
            case PlayerInteraction.GetWater:
                anim.SetBool("GetWater", true);
                break;
            case PlayerInteraction.Axe:
                anim.SetBool("Axe", true);
                break;
            case PlayerInteraction.Fertilize:
                anim.SetBool("Fertilize", true);
                break;
        }

        anim.SetFloat("MouseX", playerToMouse.x);
        anim.SetFloat("MouseY", playerToMouse.y);
        SetCanMove(false);
    }

    public void SetCanMove(bool _canMove)
    {
        canMove = _canMove;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(curPos, 1f);
    }

}
