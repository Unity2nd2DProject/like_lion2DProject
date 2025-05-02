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
    public Vector2 alphaRange = new Vector2(0.1f, 0.4f);  // 불투명도 범위
    public Vector2 yRange = new Vector2(-8f, 8f);
    public float xOffset = 10f;
    public float shadowYOffset = -3.5f;

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
        // 스폰 위치에 z=0 명시적 지정
        Vector3 spawnPos = Camera.main.transform.position + new Vector3(xOffset, Random.Range(yRange.x, yRange.y), 0);
        var prefab = cloudPrefabs[Random.Range(0, cloudPrefabs.Length)];

        // 랜덤 값들 생성
        float randomScale = Random.Range(scaleRange.x, scaleRange.y);
        float randomAlpha = Random.Range(alphaRange.x, alphaRange.y);
        float randomSpeed = Random.Range(speedRange.x, speedRange.y);

        // 구름과 그림자 생성 시 Z 포지션 확인
        var cloud = Instantiate(prefab, spawnPos, Quaternion.identity, weatherEffectsParent);
        var shadow = Instantiate(prefab, new Vector3(spawnPos.x, spawnPos.y + shadowYOffset, 0), Quaternion.identity, weatherEffectsParent);

        // Z 위치 강제 설정
        cloud.transform.position = new Vector3(cloud.transform.position.x, cloud.transform.position.y, 0);
        shadow.transform.position = new Vector3(shadow.transform.position.x, shadow.transform.position.y, 0);

        // 크기 설정
        cloud.transform.localScale = new Vector3(randomScale, randomScale, 1);
        shadow.transform.localScale = new Vector3(randomScale, randomScale, 1);

        // 구름 알파값 설정
        SpriteRenderer cloudRenderer = cloud.GetComponent<SpriteRenderer>();
        if (cloudRenderer != null)
        {
            cloudRenderer.color = new Color(1, 1, 1, randomAlpha); // 구름의 알파값 설정
        }

        // 그림자 색상 설정
        SpriteRenderer shadowRenderer = shadow.GetComponent<SpriteRenderer>();
        if (shadowRenderer != null)
        {
            shadowRenderer.color = new Color(0, 0, 0, randomAlpha);
        }

        // 속도 설정
        cloud.AddComponent<CloudMover>().speed = randomSpeed;
        shadow.AddComponent<CloudMover>().speed = randomSpeed;
    }
}
