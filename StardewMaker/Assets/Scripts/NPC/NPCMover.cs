using UnityEngine;

public class NPCMover : MonoBehaviour
{
    private Animator animator;

    [Header("Info")]
    [SerializeField] private float speed = 2f;

    [Header("Check")]
    [SerializeField] private Transform[] route;
    private int index = 0;
    [SerializeField] private NpcActionType arrivalAction = NpcActionType.None;
    [SerializeField] private bool isMoving = false;
    [SerializeField] private string teleportWp;

    public Transform defaultPosition;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetRoute(Transform[] newRoute, NpcActionType action, string _teleportWp = null)
    {
        route = newRoute;
        arrivalAction = action;
        index = 0;
        isMoving = true;
        teleportWp = _teleportWp;
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

        /*
        Transform target = route[index];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);

        Vector2 moveDir = (target.position - transform.position).normalized;
        bool stillMoving = Vector2.Distance(transform.position, target.position) > 0.05f;
        animator.SetBool("IsMoving", stillMoving);
        if (stillMoving)
        {
            animator.SetFloat("MoveX", moveDir.x);
            animator.SetFloat("MoveY", moveDir.y);
        }

        if (Vector2.Distance(transform.position, target.position) < 0.1f)
        {
            index++;

            if (index >= route.Length)
            {
                OnArrival();
            }
        }
        */
    }

    private void OnArrival()
    {
        isMoving = false;
        animator.SetBool("IsMoving", false);
        animator.SetFloat("MoveX", 0);
        animator.SetFloat("MoveY", 0);
        //Debug.Log($"[NPCMover] {gameObject.name} 도착! -> {arrivalAction}");

        switch (arrivalAction)
        {
            case NpcActionType.None:
                break;
            case NpcActionType.Teleport:
                Teleport();
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

    private void Teleport()
    {
        Transform target = WaypointManager.Instance.GetPosition(teleportWp);
        if (target != null)
        {
            transform.position = target.position;
        }
    }

    public void ResetToDefaultPosition()
    {

    }
}
