using UnityEngine;
using UnityEngine.UI;

public class WildAnimalHpBarUI : MonoBehaviour
{
    [SerializeField] private Image hpBar; 
    [SerializeField] private Vector3 offset = new Vector3(0f, 0.7f, 0f);

    private Transform target;

    public void Initialize(Transform target, int maxHP, int curHP)
    {
        this.target = target;
        transform.SetParent(target); 
        transform.localPosition = offset;
        transform.localScale = Vector3.one;
        UpdateHealthBar(curHP, maxHP);
    }

    public void UpdateHealthBar(int curHP, int maxHP)
    {
        if (hpBar != null && hpBar.type == Image.Type.Filled)
        {
            hpBar.fillAmount = (float)curHP / maxHP;
        }
        else
        {
            Debug.LogWarning("HealthBarFill is not assigned or not set to Filled type!");
        }
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position + offset;
            transform.rotation = Camera.main.transform.rotation;
        }
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
