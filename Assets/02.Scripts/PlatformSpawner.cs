using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PlatformChance
    {
        public GameObject platformPrefab; // 발판 프리팹
        public float chance;              // 발판 생성 확률 (0 ~ 1)
    }

    [System.Serializable]
    public class XPositionChance
    {
        public float xPosition;           // X 위치
        public float chance;              // X 위치 생성 확률 (0 ~ 1)
    }

    public PlatformChance[] platforms;     // 발판과 확률 배열
    public XPositionChance[] xPositions;   // X 위치와 확률 배열
    public float spawnRate = 2f;           // 발판 생성 주기
    public float fixedYSpacing = 4f;       // 고정된 수직 간격

    private float nextSpawnTime;
    private float lastYPosition;

    void Start()
    {
        lastYPosition = transform.position.y;
        nextSpawnTime = Time.time + spawnRate;
        SpawnPlatform();
    }

    void Update()
    {
        if (Time.time >= nextSpawnTime)
        {
            SpawnPlatform();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnPlatform()
    {
        // 1. 발판 종류 선택
        float totalPlatformChance = 0f;
        foreach (var platform in platforms)
        {
            totalPlatformChance += platform.chance;
        }
        float randomPlatformValue = Random.value * totalPlatformChance;
        GameObject selectedPlatform = null;
        float cumulativePlatformChance = 0f;
        foreach (var platform in platforms)
        {
            cumulativePlatformChance += platform.chance;
            if (randomPlatformValue <= cumulativePlatformChance)
            {
                selectedPlatform = platform.platformPrefab;
                break;
            }
        }

        // 2. X 위치 선택
        float totalXChance = 0f;
        foreach (var pos in xPositions)
        {
            totalXChance += pos.chance;
        }

        float randomXValue = Random.value * totalXChance;
        float selectedX = 0f;
        float cumulativeXChance = 0f;

        foreach (var pos in xPositions)
        {
            cumulativeXChance += pos.chance;
            if (randomXValue <= cumulativeXChance)
            {
                selectedX = pos.xPosition;
                break;
            }
        }

        // 3. 위치 계산 및 생성
        float fixedY = lastYPosition + fixedYSpacing;
        Vector3 spawnPosition = new Vector3(selectedX, fixedY, 0);
        Instantiate(selectedPlatform, spawnPosition, Quaternion.identity);

        lastYPosition = fixedY;
    }
}