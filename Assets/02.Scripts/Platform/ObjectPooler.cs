using UnityEngine;
using System.Collections.Generic;

public class ObjectPooler : MonoBehaviour
{
    public int poolSize = 10; // 풀 크기: 생성할 발판의 최대 개수 (인스펙터에서 설정 가능)
    private List<GameObject> platformPool; // 발판 오브젝트를 저장하는 리스트 (비활성화된 발판 재사용)

    // 풀 초기화 메서드: 발판을 미리 생성해 풀에 저장
    public void Initialize(PlatformSelector platformSelector, Transform platformsParent)
    {
        platformPool = new List<GameObject>(); // 발판 풀 리스트 초기화

        // 설정된 풀 크기만큼 발판 생성
        for (int i = 0; i < poolSize; i++)
        {
            // PlatformSelector를 사용해 랜덤 발판 선택 및 생성
            GameObject platform = Object.Instantiate(platformSelector.SelectRandomPlatform(), Vector3.zero, Quaternion.identity);
            platform.transform.SetParent(platformsParent); // Platforms를 부모로 설정 (Hierarchy 정리)
            platform.SetActive(false); // 초기에는 비활성화 상태로 설정
            platformPool.Add(platform); // 풀에 추가
        }
    }

    // 풀에서 비활성화된 발판을 반환하는 메서드
    public GameObject GetPooledPlatform(GameObject platformPrefab)
    {
        // 비활성화된 발판 찾기
        foreach (var platform in platformPool)
        {
            if (!platform.activeInHierarchy) // 비활성화 상태면 반환
            {
                return platform;
            }
        }

        // 풀 크기 초과 시 새 오브젝트 생성 금지
        if (platformPool.Count >= poolSize)
        {
            Debug.LogWarning("풀 크기(" + poolSize + ")를 초과해 더 이상 발판을 생성하지 않습니다.");
            return null; // 생성 불가 시 null 반환
        }

        // 풀 크기 미달 시 새 오브젝트 생성 (현재 로직상 실행되지 않음)
        GameObject newPlatform = Object.Instantiate(platformPrefab, Vector3.zero, Quaternion.identity);
        newPlatform.transform.SetParent(platformPrefab.transform.parent); // 부모 유지
        newPlatform.SetActive(false); // 비활성화 상태로 설정
        platformPool.Add(newPlatform); // 풀에 추가
        return newPlatform; // 새 발판 반환
    }
}