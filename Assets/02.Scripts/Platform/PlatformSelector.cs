using UnityEngine;

public class PlatformSelector : MonoBehaviour
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

    public PlatformChance[] platforms; // 발판과 확률 배열
    public XPositionChance[] xPositions; // X 위치와 확률 배열

    public GameObject SelectRandomPlatform()
    {
        if (platforms == null || platforms.Length == 0)
        {
            Debug.LogError("platforms 배열이 비어 있습니다. 기본값을 반환합니다.");
            return null; // 또는 기본 프리팹 설정 가능
        }

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

    public float SelectRandomXPosition()
    {
        if (xPositions == null || xPositions.Length == 0)
        {
            Debug.LogError("xPositions 배열이 비어 있습니다. 기본값 0을 반환합니다.");
            return 0f; // 기본값
        }

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