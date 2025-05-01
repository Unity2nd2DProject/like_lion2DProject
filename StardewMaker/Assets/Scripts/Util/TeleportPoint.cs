using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider2D))]
public class TeleportPoint : MonoBehaviour
{
    [Header("Teleport Settings")]
    [SerializeField] private Transform destinationPoint;

    [Header("Fade Settings")]
    [SerializeField] private float fadeDuration = 1f;     // 페이드 인/아웃 시간

    [Header("Camera Sync")]
    [Tooltip("카메라가 목표 위치에 얼마나 가까워야 완료로 간주할지")]
    [SerializeField] private float cameraThreshold = 0.1f;
    [Tooltip("카메라가 따라오는 최대 대기 시간")]
    [SerializeField] private float maxCameraWait = 1f;

    private bool isTeleporting = false;
    private Transform cameraTransform;

    void Start()
    {
        // Main Camera의 Transform 가져오기
        if (Camera.main != null)
            cameraTransform = Camera.main.transform;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (isTeleporting) return;

        var player = collision.GetComponent<PlayerController>();
        if (player != null && !player.justTeleported)
        {
            StartCoroutine(HandleTeleportCooldown(player));
            StartCoroutine(FadeTeleport(player));
        }
    }

    private IEnumerator HandleTeleportCooldown(PlayerController player)
    {
        player.justTeleported = true;
        yield return new WaitForSeconds(3f); // 트리거 쿨다운
        player.justTeleported = false;
    }

    private IEnumerator FadeTeleport(PlayerController player)
    {
        isTeleporting = true;

        player.SetCanMove(false);

        // 페이드 아웃
        FadeManager.Instance.FadeOut();
        yield return new WaitForSeconds(fadeDuration);

        // 플레이어 이동
        if (destinationPoint != null)
        {
            player.transform.position = destinationPoint.position;
        }

        // 카메라가 새 위치에 도착할 때까지 대기
        if (cameraTransform != null)
        {
            Vector3 targetCamPos = new Vector3(
                destinationPoint.position.x,
                destinationPoint.position.y,
                cameraTransform.position.z
            );

            float elapsed = 0f;
            // 카메라가 충분히 가까워질 때까지, 또는 maxCameraWait 초까지
            while (Vector2.Distance(
                       new Vector2(cameraTransform.position.x, cameraTransform.position.y),
                       new Vector2(targetCamPos.x, targetCamPos.y)
                   ) > cameraThreshold
                   && elapsed < maxCameraWait)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
        }

        // 페이드 인
        FadeManager.Instance.FadeIn();
        yield return new WaitForSeconds(fadeDuration);

        player.SetCanMove(true);

        isTeleporting = false;
    }
}
