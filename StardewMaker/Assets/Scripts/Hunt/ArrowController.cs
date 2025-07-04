using NUnit.Framework;
using UnityEngine;

public class ArrowController : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    [Header("Info")]
    [SerializeField] private float speed = 2f;
    [SerializeField] private float maxDistance = 6f;

    private Vector3 startPos;
    private Vector2 moveDirection;

    [Header("Sprites")]
    [SerializeField]  private Sprite downArrow;
    [SerializeField]  private Sprite leftArrow;

    private void Start()
    {
        startPos = transform.position;
    }

    private void Update()
    {
        transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(startPos, transform.position) >= maxDistance)
        {
            AttackManager.Instance.SetArrowActive(false);
            Destroy(gameObject);
        }
    }

    public void Init(Vector2 direction)
    {
        moveDirection = direction.normalized;

        spriteRenderer = GetComponent<SpriteRenderer>();

        if (moveDirection == Vector2.left)
        {
            spriteRenderer.sprite = leftArrow;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        else if (moveDirection == Vector2.right)
        {
            spriteRenderer.sprite = leftArrow;
            spriteRenderer.flipX = true;
            spriteRenderer.flipY = false;
        }
        else if (moveDirection == Vector2.down)
        {
            spriteRenderer.sprite = downArrow;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = false;
        }
        else if (moveDirection == Vector2.up)
        {
            spriteRenderer.sprite = downArrow;
            spriteRenderer.flipX = false;
            spriteRenderer.flipY = true;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("WildAnimal"))
        {
            WildAnimalController animal = collision.GetComponent<WildAnimalController>();
            if (animal != null)
            {
                Debug.Log("arrow hit!");
                animal.TakeDamage(10); 
            }
            AttackManager.Instance.SetArrowActive(false);
            Destroy(gameObject);
        }
    }

}
