using UnityEngine;

public class AttackManager : Singleton<AttackManager> // TODO : 제거
{
    [Header("Bow")]
    [SerializeField] private GameObject arrowPrefab;
    private bool isArrowActive = false;

    public void ShootArrow(Transform firePoint, Vector2 direction)
    {
        if (isArrowActive)
        {
            return;
        }

        isArrowActive = true;
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        ArrowController arrowCtrl = arrow.GetComponent<ArrowController>();
        if (arrowCtrl != null)
        {
            arrowCtrl.Init(direction);
        }
        StatManager.Instance.AddExpToSkill(TraitType.Hunting);
    }

    public void SetArrowActive(bool isActive)
    {
        isArrowActive = isActive;
    }
}
