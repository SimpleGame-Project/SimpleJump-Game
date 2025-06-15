using UnityEngine;

// 플랫폼 스폰 전체 관리 (스폰 타이밍, 위치, 풀링 등)
public class PlatformSpawnManager : MonoBehaviour
{
    [SerializeField] private PlatformSelector platformSelector;     // 발판 종류/위치 선택기
    [SerializeField] private PlatformPositioner platformPositioner; // Y 위치 계산
    [SerializeField] private PlatformPooler platformPooler;         // 풀 매니저

    [SerializeField] private float spawnRate = 2f;                  // 발판 생성 주기(초)
    [SerializeField] private float startPlatformY = 0f;             // 첫 발판 Y 위치

    private float nextSpawnTime;

    void Start()
    {
        // 시작 Y 위치에서 발판 생성 시작
        platformPositioner.Initialize(startPlatformY);
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnPlatform();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    // 발판 생성
    private void SpawnPlatform()
    {
        GameObject prefab = platformSelector.SelectRandomPlatform(); // 발판 종류 선택
        float x = platformSelector.SelectRandomXPosition();         // X 위치 선택
        Vector3 spawnPos = platformPositioner.CalculateNextSpawnPosition(x);

        JumpPlatform platform = platformPooler.GetPooledPlatform();
        if (platform != null)
        {
            platform.transform.position = spawnPos;
        }
        // platform == null 이면 풀 최대치라 생성 안 됨
    }
}
