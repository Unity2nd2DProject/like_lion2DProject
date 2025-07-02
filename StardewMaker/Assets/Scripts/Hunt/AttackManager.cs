using UnityEngine;

public class AttackManager : Singleton<AttackManager>
{
    [Header("Bow")]
    [SerializeField] private GameObject arrowPrefab;

    public void ShootArrow(Transform firePoint, Vector2 direction)
    {
        GameObject arrow = Instantiate(arrowPrefab, firePoint.position, Quaternion.identity);

        ArrowController arrowCtrl = arrow.GetComponent<ArrowController>();
        if (arrowCtrl != null)
        {
            arrowCtrl.Init(direction);
        }
    }
}
