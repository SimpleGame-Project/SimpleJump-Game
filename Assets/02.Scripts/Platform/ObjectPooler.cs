using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public int poolSize = 10; // 오브젝트 풀 크기 (설정 가능)
    private List<GameObject> platformPool;

    public void Initialize(PlatformSelector platformSelector, Transform platformsParent)
    {
        platformPool = new List<GameObject>();

        // 풀 크기만큼 발판 초기 생성
        for (int i = 0; i < poolSize; i++)
        {
            GameObject platform = Object.Instantiate(platformSelector.SelectRandomPlatform(), Vector3.zero, Quaternion.identity);
            platform.transform.SetParent(platformsParent); // Platforms를 부모로 설정
            platform.SetActive(false);
            platformPool.Add(platform);
        }
    }

    public GameObject GetPooledPlatform(GameObject platformPrefab)
    {
        // 비활성화된 발판 찾기
        foreach (var platform in platformPool)
        {
            if (!platform.activeInHierarchy)
            {
                return platform;
            }
        }

        // 풀 크기 초과 시 새 오브젝트 생성 금지
        if (platformPool.Count >= poolSize)
        {
            Debug.LogWarning("풀 크기(" + poolSize + ")를 초과해 더 이상 발판을 생성하지 않습니다.");
            return null; // 더 이상 생성하지 않음
        }

        // 풀 크기 미달 시 새 오브젝트 생성 (실제로는 이 코드가 실행되지 않도록 제한)
        GameObject newPlatform = Object.Instantiate(platformPrefab, Vector3.zero, Quaternion.identity);
        newPlatform.transform.SetParent(platformPrefab.transform.parent); // 부모 유지
        newPlatform.SetActive(false);
        platformPool.Add(newPlatform);
        return newPlatform;
    }
}