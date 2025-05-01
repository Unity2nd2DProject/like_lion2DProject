using UnityEngine;
using Unity.Cinemachine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class TeleportPoint : MonoBehaviour
{
    [Header("Teleport Destination")]
    [SerializeField] private Transform destinationPoint;

    [Header("Camera Confiner")]
    [SerializeField] private CinemachineConfiner2D confiner;
    [SerializeField] private Collider2D farmCollider;
    [SerializeField] private Collider2D townCollider;
    [SerializeField] private bool goToTown = true;

    private bool isTeleporting = false;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTeleporting) return;

        PlayerController player = collision.GetComponent<PlayerController>();
        if (player != null && !player.justTeleported)
        {
            StartCoroutine(HandleTeleportCooldown(player));
            Teleport(player);
        }
    }

    private IEnumerator HandleTeleportCooldown(PlayerController player)
    {
        player.justTeleported = true;
        yield return new WaitForSeconds(3f); // 쿨다운
        player.justTeleported = false;
    }

    private void Teleport(PlayerController player)
    {
        if (isTeleporting) return;
        isTeleporting = true;
        Debug.Log("Confiner Bounds: " + confiner.BoundingShape2D.bounds);

        FadeManager.Instance.FadeOut(() =>
        {
            // 플레이어 위치 이동
            if (destinationPoint != null)
            {
                player.transform.position = destinationPoint.position;
            }

            // 카메라 Confiner 변경
            if (confiner != null)
            {
                confiner.BoundingShape2D = goToTown ? townCollider : farmCollider;
                confiner.InvalidateBoundingShapeCache();
            }

            // 페이드 인
            FadeManager.Instance.FadeIn();

            isTeleporting = false;
        });
    }
}