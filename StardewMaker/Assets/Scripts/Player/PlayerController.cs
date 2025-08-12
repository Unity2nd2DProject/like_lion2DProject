using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

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
    Fertilize,
    PickFruit,
    Shoot
}

public class PlayerController : Singleton<PlayerController>
{
    private string TAG = "[PlayerController]";
    private UserInputManager inputManager;

    public Animator anim { get; private set; }
    public Rigidbody2D rb { get; private set; }
    private PlayerAttackCollider playerAttackCollider;

    [Header("Move")]
    private Vector2 mouseWorldPos;
    private Vector2 moveInput, move;
    private Vector2 lastMove;
    private Vector2 playerToMouse;
    public float moveSpeed = 5f;
    private Vector2 curPos;
    private bool canMove = true;
    private bool isStunned = false;
    private float stunTimer = 0f;
    public bool justTeleported = false;
    private bool isTeleporting = false;
    private Transform cameraTransform;

    [Header("Attack")]
    [SerializeField] private int maxHp = 100;
    [SerializeField] private int curHp;
    [SerializeField] private Transform leftPoint;
    [SerializeField] private Transform rightPoint;
    [SerializeField] private Transform downPoint;
    [SerializeField] private Transform upPoint;

    [Header("Interact Check")]
    [SerializeField] private FarmLand curFarmLand;
    [SerializeField] private Pond curPond;
    [SerializeField] private Tree curTree;
    [SerializeField] private Bush curBush;
    [SerializeField] private ItemData curItem;
    [SerializeField] private MapArea curMapArea;
    private bool isInteracting = false;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponentInChildren<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerAttackCollider = GetComponent<PlayerAttackCollider>();

        if (Camera.main != null)
        {
            cameraTransform = Camera.main.transform;
        }
    }

    private void Start()
    {
        curHp = maxHp;
        //PlayerHpBarUI.Instance.Initialize(maxHp, curHp);
    }

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

        if (isStunned)
        {
            stunTimer -= Time.deltaTime;
            if (stunTimer <= 0f)
            {
                isStunned = false;
            }
            return;
        }

        PlayerMoveInput();
        SpaceInput();
        // ESCInput();
        ZInput();
        XInput();
        IInput();
        QInput();
        F1Input();
        NInput();
        MouseLeftInput();
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + move * moveSpeed * Time.fixedDeltaTime);
        curPos = rb.position;

        mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 direction = mouseWorldPos - curPos;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            playerToMouse = direction.x > 0 ? Vector2.right : Vector2.left;
        }
        else
        {
            playerToMouse = direction.y > 0 ? Vector2.up : Vector2.down;
        }

        if (!SceneManager.GetActiveScene().name.Contains("Home"))
        {
            curMapArea = MapManager.Instance.GetArea(transform.position);
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

    // private void ESCInput()
    // {
    //     if (inputManager.inputActions.Player.ESC.WasPressedThisFrame())
    //     {
    //         Debug.Log($"{TAG} ESCInput IsPressed. UI 인풋으로 전환");
    //         GameManager.Instance.SetGameState(TAG, GameState.UI);
    //     }
    // }

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

    private void QInput()
    {
        if (inputManager.inputActions.Player.Q.WasPressedThisFrame())
        {
            QuestUI.Instance.ToggleQuestPanel();
        }
    }

    private void NInput() // NextDay (Test)
    {
        if (inputManager.inputActions.Player.N.WasPressedThisFrame())
        {
            TimeManager.Instance.AdvanceDay();
        }
    }

    private void F1Input() // ending test
    {
        if (inputManager.inputActions.Player.F1.WasPressedThisFrame())
        {
            //SetInteractAnimation(PlayerInteraction.Shoot);
        }
    }

    private void MouseLeftInput()
    {
        if (inputManager.inputActions.Player.MouseLeft.WasPressedThisFrame())
        {
            Collider2D mouseHit = Physics2D.OverlapPoint(mouseWorldPos);
            Collider2D[] playerHits = Physics2D.OverlapCircleAll(curPos, 1f);

            if (UserInputManager.Instance.inputActions.Player.Move.ReadValue<Vector2>() == Vector2.zero)
            {
                curItem = InventoryManager.Instance.GetQuickSlotCurrentSelectedItem();
                playerAttackCollider.SetCurItem(curItem);

                if (curMapArea == MapArea.Forest)
                {
                    if (curItem.name == "ToolBow")
                    {
                        SetInteractAnimation(PlayerInteraction.Shoot);
                    }
                    else if (curItem.name == "ToolAxe")
                    {
                        SetInteractAnimation(PlayerInteraction.Axe);
                    }
                    else
                    {
                        InteractWithObject(mouseHit, playerHits);
                    }
                }
                else
                {
                    InteractWithObject(mouseHit, playerHits);
                }
            }            
        }
    }

    private void InteractWithObject(Collider2D mouseHit, Collider2D[] playerHits)
    {
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
                                if (curFarmLand.CanFertilze() && StaminaManager.Instance.CanConsumeStamina())
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
                                    if (curFarmLand.CanPick() && StaminaManager.Instance.CanConsumeStamina())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Pick);
                                    }
                                }
                            }
                            else if (curItem.name == "ToolWateringCan")
                            {
                                if (curFarmLand != null)
                                {
                                    if (curFarmLand.CanWater() && StaminaManager.Instance.CanConsumeStamina())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Water);
                                    }
                                }
                                else if (curPond != null && StaminaManager.Instance.CanConsumeStamina())
                                {
                                    SetInteractAnimation(PlayerInteraction.GetWater);
                                }
                            }
                            else if (curItem.name == "ToolAxe")
                            {
                                if (curTree != null && StaminaManager.Instance.CanConsumeStamina())
                                {
                                    SetInteractAnimation(PlayerInteraction.Axe);
                                }
                            }
                            else if (curItem.name == "ToolFishingRod")
                            {
                                if (curPond != null && StaminaManager.Instance.CanConsumeStamina())
                                {
                                    SetInteractAnimation(PlayerInteraction.Fish);
                                }
                            }
                            else if (curItem.name == "ToolGlove")
                            {
                                if (curFarmLand != null)
                                {
                                    var crop = CropManager.Instance.GetCropAt(curFarmLand.GetPosition());

                                    if (crop != null && crop.cropData.id == 7 && TimeManager.Instance.IsLastDay())
                                    {
                                        EndingResult ending = CropManager.Instance.GetEndingResult();
                                        GameManager.Instance.GoToEnding(ending);
                                        return;
                                    }

                                    if (crop != null && crop.cropData.id != 7 && curFarmLand.CanHarvest())
                                    {
                                        SetInteractAnimation(PlayerInteraction.Harvest);
                                    }
                                } else if (curBush != null && curBush.CanPick())
                                {
                                    SetInteractAnimation(PlayerInteraction.PickFruit);
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
        curBush = null;

        Debug.Log("Hit object: " + hit.gameObject.name);

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
        else if (hit.TryGetComponent(out Bush bush))
        {
            curBush = bush;
        }
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
        if (curTree != null)
        {
            curTree.Chop();
            return;
        }
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

    public void PickFruit()
    {
        curBush.PickFruit();
    }

    public void ShootArrow()
    {
        if (playerToMouse == Vector2.left)
        {
            AttackManager.Instance.ShootArrow(leftPoint, playerToMouse);
        } 
        else if (playerToMouse == Vector2.right)
        {
            AttackManager.Instance.ShootArrow(rightPoint, playerToMouse);
        }
        else if (playerToMouse == Vector2.down)
        {
            AttackManager.Instance.ShootArrow(downPoint, playerToMouse);
        }
        else if (playerToMouse == Vector2.up)
        {
            AttackManager.Instance.ShootArrow(upPoint, playerToMouse);
        }
    }

    private void SetInteractAnimation(PlayerInteraction interaction)
    {
        if (isInteracting)
        {
            return;
        }
        isInteracting = true;
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
            case PlayerInteraction.PickFruit:
                anim.SetBool("PickFruit", true);
                break;
            case PlayerInteraction.Shoot:
                anim.SetBool("Shoot", true);
                break;
        }

        anim.SetFloat("MouseX", playerToMouse.x);
        anim.SetFloat("MouseY", playerToMouse.y);
        SetCanMove(false);
    }

    public void OnFinishTrigger()
    {
        isInteracting = false;
        SetCanMove(true);
    }

    public void SetCanMove(bool _canMove)
    {
        canMove = _canMove;
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        HpUI.Instance.UpdateHearts(curHp);
    }


    public void TeleportToTown()
    {
        if (!isTeleporting)
        {
            Transform targetPoint = WaypointManager.Instance.GetPosition("twp0");
            StartCoroutine(FadeTeleport(targetPoint));
        }
    }

    private IEnumerator FadeTeleport(Transform destination)
    {
        isTeleporting = true;

        // 이동 및 애니메이션 차단
        SetCanMove(false);
        if (anim != null)
        {
            anim.SetBool("Move", false);
            anim.speed = 0f;
        }

        UserInputManager.Instance.inputActions.Player.Disable();

        // 이동 벡터 수동 초기화
        var moveField = typeof(PlayerController).GetField("move", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        if (moveField != null)
        {
            moveField.SetValue(this, Vector2.zero);
        }

        // 페이드 아웃
        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(1f);

        // 플레이어 이동
        transform.position = destination.position;

        // 카메라 이동 대기
        if (cameraTransform != null)
        {
            Vector3 targetCamPos = new Vector3(destination.position.x, destination.position.y, cameraTransform.position.z);
            float elapsed = 0f;
            while (Vector2.Distance(cameraTransform.position, targetCamPos) > 0.1f && elapsed < 1f)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        // 페이드 인
        FadeManager.Instance.FadeIn();
        yield return new WaitForSeconds(1f);

        // 복구
        if (anim != null)
        {
            anim.SetBool("Move", false);
            anim.speed = 1f;
        }
        UserInputManager.Instance.inputActions.Player.Enable();
        SetCanMove(true);

        isTeleporting = false;
    }

    public void Stun(float duration = 0.3f)
    {
        isStunned = true;
        stunTimer = duration;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(curPos, 1f);
    }
}
