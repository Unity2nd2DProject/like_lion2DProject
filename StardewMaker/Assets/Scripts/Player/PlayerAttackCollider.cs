using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackCollider : MonoBehaviour
{
    [SerializeField] private CircleCollider2D leftCollider;
    [SerializeField] private CircleCollider2D rightCollider;
    [SerializeField] private CircleCollider2D upCollider;
    [SerializeField] private CircleCollider2D downCollider;
    [SerializeField] private float attackCooldown = 0.5f;

    private float nextDamageTime = 0f;
    private ItemData curItem;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("WildAnimal"))
        {
            return;
        }

        if (Time.time < nextDamageTime)
        {
            return;
        }

        var animal = collision.GetComponent<WildAnimalController>();
        if (animal != null && curItem != null)
        {
            if (curItem.name == "ToolAxe")
            {
                animal.TakeDamage(5);
            }
            else if (curItem.name == "ToolBow")
            {
                animal.TakeDamage(10);
            }

            nextDamageTime = Time.time + attackCooldown;
        }
    }

    public void SetCurItem(ItemData _curItem)
    {
        curItem = _curItem;
    }
}
