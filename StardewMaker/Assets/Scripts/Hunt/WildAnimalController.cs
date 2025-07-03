using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum AnimalType { Rabbit, Deer, WildBoar, Bear }
public enum AnimalState { Idle, Walk, Chase, Attack }
public class WildAnimalController : MonoBehaviour
{
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    [Header("Info")]
    [SerializeField] private AnimalType animalType;
    [SerializeField] private int maxHP;
    [SerializeField] private int curHp;
    [SerializeField] private bool isDead = false;

    [Header("State")]
    [SerializeField] private AnimalState curState = AnimalState.Idle;
    [SerializeField] private float detectRange = 5f;
    [SerializeField] private float attackRange = 1.5f;
    [SerializeField] private float hitRange = 1.5f;
    [SerializeField] private float attackCooldown = 3f;
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float stateDuration = 3f;

    [Header("UI")]
    [SerializeField] private WildAnimalHpBarUI hpBarPrefab; 
    private WildAnimalHpBarUI hpBarInstance;

    [Header("Check")]
    [SerializeField] private float stateTimer;
    [SerializeField] private float distanceToPlayer;
    [SerializeField] private int lastFacingDirection = 1;
    [SerializeField] private Vector3 walkDirection;
    [SerializeField] private Vector3 dirToPlayer;
    [SerializeField] private float lastAttackTime = -Mathf.Infinity;
    [SerializeField] private List<DropItem> dropItems = new List<DropItem>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    private void Start()
    {
        curHp = maxHP;
        ChangeState(AnimalState.Idle);
        WildAnimalManager.Instance.RegisterAnimal(animalType);
        SetupDropItems();
        SetupHealthBar();
    }

    private void SetupHealthBar()
    {
        if (hpBarPrefab != null)
        {
            Canvas canvas = FindFirstObjectByType<Canvas>();
            if (canvas == null)
            {
                Debug.LogError("Canvas not found in scene!");
                return;
            }

            hpBarInstance = Instantiate(hpBarPrefab, canvas.transform);
            hpBarInstance.Initialize(transform, maxHP, curHp);
        }
        else
        {
            Debug.LogWarning("hpBarPrefab not assigned in WildAnimalController!");
        }
    }


    private void SetupDropItems()
    {
        dropItems = WildAnimalManager.Instance.GetDropItems(animalType);
    }

    private void Update()
    {
        if (isDead)
        {
            return;
        }

        distanceToPlayer = Vector3.Distance(transform.position, PlayerController.Instance.transform.position);
        dirToPlayer = PlayerController.Instance.transform.position - transform.position;

        switch (curState)
        {
            case AnimalState.Idle:
                IdleUpdate();
                break;
            case AnimalState.Walk:
                WalkUpdate();
                break;
            case AnimalState.Chase:
                ChaseUpdate();
                break;
            case AnimalState.Attack:
                AttackUpdate();
                break;
        }

    }

    private void IdleUpdate()
    {
        if (distanceToPlayer <= detectRange && Time.time >= lastAttackTime + attackCooldown)
        {
            ChangeState(AnimalState.Chase);
            return;
        }

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            ChangeState(AnimalState.Walk);
            return;
        }

    }

    private void WalkUpdate()
    {
        if (distanceToPlayer <= detectRange && Time.time >= lastAttackTime + attackCooldown)
        {
            ChangeState(AnimalState.Chase);
            return;
        }

        stateTimer -= Time.deltaTime;
        if (stateTimer <= 0f)
        {
            ChangeState(AnimalState.Idle);
            return;
        }

        transform.position += walkDirection * moveSpeed * Time.deltaTime;
    }
    
    private void ChaseUpdate()
    {
        if (distanceToPlayer <= attackRange)
        {
            ChangeState(AnimalState.Attack);
            return;
        }

        transform.position += dirToPlayer * moveSpeed * 1.1f * Time.deltaTime;
    }

    private void AttackUpdate()
    {
        if (distanceToPlayer > detectRange)
        {
            ChangeState(AnimalState.Idle);
            return;
        }

        switch (animalType)
        {
            case AnimalType.Rabbit:
                RabbitAttack();
                break;
            case AnimalType.Deer:
                DeerAttack();
                break;
            case AnimalType.WildBoar:
                WildBoarAttack();
                break;
            case AnimalType.Bear:
                BearAttack();
                break;
        }
    }

    private void RabbitAttack()
    {
        transform.position += -dirToPlayer * moveSpeed * Time.deltaTime;
    }

    private void DeerAttack()
    {
        // animation finished
        if (distanceToPlayer < hitRange)
        {
            Debug.Log("Player took damage by deer!");
            PlayerController.Instance.TakeDamage(5);
            ChangeState(AnimalState.Idle);
            return;
        }
    }

    private void WildBoarAttack()
    {
        transform.position += dirToPlayer * moveSpeed * 1.2f * Time.deltaTime;

        if (distanceToPlayer < hitRange)
        {
            Debug.Log("Player took damage by wildboar!");
            PlayerController.Instance.TakeDamage(7);
            PlayerController.Instance.Stun();
            ChangeState(AnimalState.Idle);
            return;
        }
    }

    private void BearAttack()
    {
        // animation finished
        if (distanceToPlayer < hitRange)
        {
            Debug.Log("Player took damage by Bear!");
            PlayerController.Instance.TakeDamage(10);
            PlayerController.Instance.Stun();
            ChangeState(AnimalState.Idle);
            return;
        }
    }

    private void ChangeState(AnimalState newState)
    {
        curState = newState;

        switch (curState)
        {
            case AnimalState.Idle:
                stateTimer = 0f;
                SetFacingDirection();
                SetAnimation(false, false);
                break;
            case AnimalState.Walk:
                stateTimer = Random.Range(2f, 4f);
                SetRandomWalkDirection();
                SetAnimation(true, false);
                break;
            case AnimalState.Chase:
                SetAttackFacingDrection();
                break;
            case AnimalState.Attack:
                if (animalType != AnimalType.Rabbit)
                {
                    lastAttackTime = Time.time;
                }
                SetAnimation(false, true);
                break;
        }
    }

    private void SetAnimation(bool isWalk, bool isAttack)
    {
        animator.SetBool("Walk", isWalk);
        animator.SetBool("Attack", isAttack);
    }

    private void SetFacingDirection()
    {
        bool faceRight = Random.value < 0.5f;
        lastFacingDirection = faceRight ? 1 : 0;

        spriteRenderer.flipX = faceRight;
    }

    private void SetAttackFacingDrection()
    {
        if (animalType == AnimalType.Rabbit)
        {
            spriteRenderer.flipX = dirToPlayer.x > 0f ? false : true;
        }
        else
        {
            spriteRenderer.flipX = dirToPlayer.x < 0f ? false : true;
        }
    }

    private void SetRandomWalkDirection()
    {
        List<Vector3> directions = new List<Vector3>();

        if (lastFacingDirection == 1) 
        {
            directions.Add(Vector3.right);                       
            directions.Add((Vector3.right + Vector3.up).normalized);
            directions.Add((Vector3.right + Vector3.down).normalized);   
        }
        else
        {
            directions.Add(Vector3.left);                        
            directions.Add((Vector3.left + Vector3.up).normalized); 
            directions.Add((Vector3.left + Vector3.down).normalized);    
        }

        walkDirection = directions[Random.Range(0, directions.Count)];
    }

    public void TakeDamage(int damage)
    {
        curHp -= damage;
        hpBarInstance.UpdateHealthBar(curHp, maxHP);

        if (curHp <= 0 && !isDead)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
        DropItems();
        WildAnimalManager.Instance.UnregisterAnimal(animalType);
        Destroy(gameObject);
        hpBarInstance.Destroy();
    }

    private void DropItems()
    {
        foreach(var dropItem in dropItems)
        {
            if (Random.value <= dropItem.dropChance)
            {
                InventoryManager.Instance.AddItem(dropItem.itemData, dropItem.quantity);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectRange);
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        ReverseDirection();
    //    }
    //}

    private void ReverseDirection()
    {
        switch (curState)
        {
            case AnimalState.Walk:
                walkDirection = -walkDirection;
                break;
            case AnimalState.Chase:
                dirToPlayer = -dirToPlayer;
                break;
        }
        spriteRenderer.flipX = !spriteRenderer.flipX;
    }

}
