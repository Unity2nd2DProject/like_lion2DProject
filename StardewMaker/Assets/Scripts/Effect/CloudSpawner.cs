using UnityEngine;

public class CloudSpawner : MonoBehaviour
{
    [Header("구름 생성 설정")]
    public GameObject[] cloudPrefabs;
    public Transform weatherEffectsParent;  // Inspector에서 할당할 부모 오브젝트
    public float spawnInterval = 6f;
    public float baseSpeed = 0.5f;
    public Vector2 speedRange = new Vector2(0.1f, 0.3f);  // 속도 범위
    public Vector2 scaleRange = new Vector2(0.2f, 0.7f);  // 크기 범위

    public Vector2 yRange = new Vector2(-8f, 8f);
    public float xOffset = 10f;

    private float timer;
    private bool isFirstSpawn = true;

    void Start()
    {
        // 시작하자마자 첫 구름 생성
        SpawnCloud();
        isFirstSpawn = false;
    }

    void Update()
    {
        if (!isFirstSpawn)  // 첫 생성 이후에만 타이머 적용
        {
            timer += Time.deltaTime;
            if (timer >= spawnInterval)
            {
                SpawnCloud();
                timer = 0f;
            }
        }
    }

    void SpawnCloud()
    {
        // 메인 카메라의 화면 크기 계산
        Camera mainCamera = Camera.main;
        float height = 2f * mainCamera.orthographicSize;
        float width = height * mainCamera.aspect;

        Vector3 spawnPos;

        // 생성 위치 랜덤 선택 (0: 위, 1: 오른쪽, 2: 아래)
        int spawnType = Random.Range(0, 3);

        // 범위를 3분의 1로 줄임
        float restrictedWidth = width / 3;
        float restrictedHeight = height / 3;

        switch (spawnType)
        {
            case 0: // 위쪽에서 생성
                float spawnX = Random.Range(-restrictedWidth, restrictedWidth);
                spawnPos = mainCamera.transform.position + new Vector3(spawnX, height / 2 + xOffset, 1);
                break;

            case 1: // 오른쪽에서 생성
                float spawnY = Random.Range(-restrictedHeight, restrictedHeight);
                spawnPos = mainCamera.transform.position + new Vector3(width / 2 + xOffset, spawnY, 1);
                break;

            default: // 아래쪽에서 생성
                spawnX = Random.Range(-restrictedWidth, restrictedWidth);
                spawnPos = mainCamera.transform.position + new Vector3(spawnX, -height / 2 - xOffset, 1);
                break;
        }

        // 프리팹 선택 및 생성
        var prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];
        var cloud = Instantiate(prefab, spawnPos, Quaternion.identity, weatherEffectsParent);

        // 랜덤 크기 설정
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        cloud.transform.localScale = new Vector3(randomScale, randomScale, 1);
    }
}
