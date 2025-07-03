using UnityEngine;
using UnityEngine.UI;

public class PlayerHpBarUI : MonoBehaviour
{
    public static PlayerHpBarUI Instance;
    [SerializeField] private Image hpBar;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    public void Initialize(int maxHP, int curHP)
    {
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
}
