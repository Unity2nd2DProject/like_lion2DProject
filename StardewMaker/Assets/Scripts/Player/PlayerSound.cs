using UnityEngine;

public class PlayerSound : MonoBehaviour
{
    [SerializeField] private string walkSFXName = "Walk";
    [SerializeField] private float stepInterval = 0.4f;
    [SerializeField] private float minMoveDistance = 0.01f;

    private Vector2 lastPosition;
    private float timer = 0f;

    private void Start()
    {
        lastPosition = new Vector2(transform.position.x, transform.position.y);
    }

    private void Update()
    {
        // 페이드 중이면 소리를 재생하지 않음
        if (FadeManager.Instance != null && FadeManager.Instance.IsFading)
        {
            return;
        }

        timer -= Time.deltaTime;

        Vector2 currentPosition = new Vector2(transform.position.x, transform.position.y);
        float moved = Vector2.Distance(currentPosition, lastPosition);

        if (moved > minMoveDistance && timer <= 0f)
        {
            SoundManager.Instance.PlaySFX(walkSFXName);
            timer = stepInterval;
        }

        lastPosition = currentPosition;
    }
}