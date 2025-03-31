using UnityEngine;

public class PlatformSpawner2 : MonoBehaviour
{
    [System.Serializable]
    public class PlatformChance
    {
        public GameObject platformPrefab; // 발판 프리팹
        public float chance;             // 발판 생성 확률 (0 ~ 1)
    }

    [System.Serializable]
    public class XPositionChance
    {
        public float xPosition;          // X 위치
        public float chance;            // X 위치 생성 확률 (0 ~ 1)
    }

    public PlatformChance[] platforms;   // 발판과 확률 배열
    public XPositionChance[] xPositions; // X 위치와 확률 배열
    public float minHeight = 10f;        // 발판 생성 최소 높이 (y 좌표)
    public float maxYSpacing = 3f;       // 최대 수직 간격 (점프 난이도 조절)
    public float minYSpacing = 1f;       // 최소 수직 간격

    private float lastYPosition;

    void Start()
    {
        lastYPosition = transform.position.y;
        SpawnPlatformUntilHeight(); // 시작 시 발판 생성
    }

    void SpawnPlatformUntilHeight()
    {
        // 최소 높이(minHeight)까지 발판 생성
        while (lastYPosition < minHeight)
        {
            SpawnSinglePlatform();
        }
    }

    void SpawnSinglePlatform()
    {
        // 1. 발판 종류 선택 (기존 로직 유지)
        GameObject selectedPlatform = SelectRandomPlatform();

        // 2. X 위치 선택 (기존 로직 유지)
        float selectedX = SelectRandomXPosition();

        // 3. Y 위치 계산 (랜덤 간격으로 생성)
        float randomYSpacing = Random.Range(minYSpacing, maxYSpacing);
        float newY = lastYPosition + randomYSpacing;

        // 4. 발판 생성
        Vector3 spawnPosition = new Vector3(selectedX, newY, 0);
        Instantiate(selectedPlatform, spawnPosition, Quaternion.identity);

        lastYPosition = newY; // 마지막 Y 위치 업데이트
    }

    GameObject SelectRandomPlatform()
    {
        float totalChance = 0f;
        foreach (var platform in platforms)
        {
            totalChance += platform.chance;
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var platform in platforms)
        {
            cumulativeChance += platform.chance;
            if (randomValue <= cumulativeChance)
            {
                return platform.platformPrefab;
            }
        }

        return platforms[0].platformPrefab; // 기본값
    }

    float SelectRandomXPosition()
    {
        float totalChance = 0f;
        foreach (var pos in xPositions)
        {
            totalChance += pos.chance;
        }

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var pos in xPositions)
        {
            cumulativeChance += pos.chance;
            if (randomValue <= cumulativeChance)
            {
                return pos.xPosition;
            }
        }

        return xPositions[0].xPosition; // 기본값
    }
}