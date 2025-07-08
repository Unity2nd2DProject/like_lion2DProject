using UnityEngine;

public class NPCMover : MonoBehaviour
{
    [Header("Info")]
    [SerializeField] private float speed = 2f;

    [Header("Check")]
    [SerializeField] private Transform[] route;
    private int index = 0;
    [SerializeField] private NpcActionType arrivalAction = NpcActionType.None;
    [SerializeField] private bool isMoving = false;

    public void SetRoute(Transform[] newRoute, NpcActionType action)
    {
        route = newRoute;
        arrivalAction = action;
        index = 0;
        isMoving = true;
    }

    public void ClearRoute()
    {
        route = null;
        index = 0;
        isMoving = false;
        arrivalAction = NpcActionType.None;
    }

    private void Update()
    {
        if (!isMoving || route == null || route.Length == 0 || index >= route.Length)
        {
            return;
        }

        Transform target = route[index];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            index++;

            if (index >= route.Length)
            {
                OnArrival();
            }
        }
    }

    private void OnArrival()
    {
        isMoving = false;
        Debug.Log($"[NPCMover] {gameObject.name} 도착! -> {arrivalAction}");

        switch (arrivalAction)
        {
            case NpcActionType.None:

                break;
            case NpcActionType.Idle:
                break;
            case NpcActionType.Walk:
                break;
            case NpcActionType.Fish:
                break;
            case NpcActionType.Chat:
                break;
        }
    }

}
