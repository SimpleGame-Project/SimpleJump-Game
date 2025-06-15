using UnityEngine;
using System.Linq;

// ScriptableObject에서 발판 프리팹과 X 위치를 확률적으로 선택
public class PlatformSelector : MonoBehaviour
{
    [SerializeField] private PlatformSpawnData spawnData; // 발판 종류/확률/X위치 데이터

    // 확률적으로 발판 프리팹 선택
    public GameObject SelectRandomPlatform()
    {
        float totalChance = spawnData.platforms.Sum(p => p.chance);
        if (totalChance <= 0)
        {
            Debug.LogError("플랫폼 확률 총합이 0 이하입니다.");
            return null;
        }
        float randomValue = Random.value * totalChance;
        float cumulative = 0f;
        foreach (var p in spawnData.platforms)
        {
            cumulative += p.chance;
            if (randomValue <= cumulative)
                return p.platformPrefab;
        }
        return spawnData.platforms[0].platformPrefab;
    }

    // 확률적으로 X 위치 선택
    public float SelectRandomXPosition()
    {
        float totalChance = spawnData.xPositions.Sum(x => x.chance);
        if (totalChance <= 0)
        {
            Debug.LogError("X 위치 확률 총합이 0 이하입니다.");
            return 0f;
        }
        float randomValue = Random.value * totalChance;
        float cumulative = 0f;
        foreach (var x in spawnData.xPositions)
        {
            cumulative += x.chance;
            if (randomValue <= cumulative)
                return x.xPosition;
        }
        return spawnData.xPositions[0].xPosition;
    }
}
