using UnityEngine;

public class PlatformSelector : MonoBehaviour
{
    // 발판 프리팹과 확률을 정의하는 클래스
    [System.Serializable]
    public class PlatformChance
    {
        public GameObject platformPrefab; // 발판 프리팹 (인스펙터에서 설정)
        public float chance;              // 발판 생성 확률 (0 ~ 1)
    }

    // X 위치와 확률을 정의하는 클래스
    [System.Serializable]
    public class XPositionChance
    {
        public float xPosition;           // X 위치 (게임 내 설정값)
        public float chance;              // X 위치 생성 확률 (0 ~ 1)
    }

    public PlatformChance[] platforms; // 발판과 확률 배열 (인스펙터에서 설정)
    public XPositionChance[] xPositions; // X 위치와 확률 배열 (인스펙터에서 설정)

    // 확률 기반으로 랜덤 발판 선택
    public GameObject SelectRandomPlatform()
    {
        if (platforms == null || platforms.Length == 0) // 배열이 비어있으면 오류 처리
        {
            Debug.LogError("platforms 배열이 비어 있습니다. 기본값을 반환합니다.");
            return null; // 또는 기본 프리팹 설정 가능
        }

        // 총 확률 계산
        float totalChance = 0f;
        foreach (var platform in platforms)
        {
            totalChance += platform.chance;
        }

        // 랜덤 값 생성 및 발판 선택
        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var platform in platforms)
        {
            cumulativeChance += platform.chance;
            if (randomValue <= cumulativeChance) // 누적 확률 기준으로 선택
            {
                return platform.platformPrefab;
            }
        }

        return platforms[0].platformPrefab; // 기본값 반환
    }

    // 확률 기반으로 랜덤 X 위치 선택
    public float SelectRandomXPosition()
    {
        if (xPositions == null || xPositions.Length == 0) // 배열이 비어있으면 오류 처리
        {
            Debug.LogError("xPositions 배열이 비어 있습니다. 기본값 0을 반환합니다.");
            return 0f; // 기본값
        }

        // 총 확률 계산
        float totalChance = 0f;
        foreach (var pos in xPositions)
        {
            totalChance += pos.chance;
        }

        // 랜덤 값 생성 및 X 위치 선택
        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var pos in xPositions)
        {
            cumulativeChance += pos.chance;
            if (randomValue <= cumulativeChance) // 누적 확률 기준으로 선택
            {
                return pos.xPosition;
            }
        }

        return xPositions[0].xPosition; // 기본값 반환
    }
}