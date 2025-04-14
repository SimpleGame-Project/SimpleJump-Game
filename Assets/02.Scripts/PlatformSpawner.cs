using UnityEngine;
using System.Collections.Generic;

// 발판을 생성하고 관리하는 스크립트
public class PlatformSpawner : MonoBehaviour
{
    // 발판 프리팹과 생성 확률을 정의하는 직렬화 클래스
    [System.Serializable]
    public class PlatformChance
    {
        public GameObject platformPrefab; // 발판 프리팹
        public float chance; // 해당 발판이 선택될 확률
    }

    // 발판의 X 위치와 생성 확률을 정의하는 직렬화 클래스
    [System.Serializable]
    public class XPositionChance
    {
        public float xPosition; // 발판이 생성될 X 좌표
        public float chance; // 해당 X 위치가 선택될 확률
    }

    [SerializeField] private PlatformChance[] platforms; // 발판 프리팹과 확률 배열
    [SerializeField] private XPositionChance[] xPositions; // X 위치와 확률 배열
    [SerializeField] private float fixedYSpacing = 4f; // 발판 간 Y축 간격
    [SerializeField] private float spawnThreshold = 5f; // 새로운 발판 생성을 위한 플레이어 Y 위치 임계값
    [SerializeField] private Transform player; // 플레이어 Transform 참조
    [SerializeField] private ItemSpawner itemSpawner; // 아이템 생성기 참조
    [SerializeField] private int initialPoolSize = 10; // 초기 오브젝트 풀 크기

    private float highestPlayerY; // 플레이어가 도달한 최고 Y 위치
    private float nextSpawnY; // 다음 발판이 생성될 Y 위치
    private Queue<GameObject> platformPool = new Queue<GameObject>(); // 비활성 발판을 저장하는 오브젝트 풀

    // 초기화 메서드 실행
    void Start()
    {
        InitializePlayerReference();
        InitializePlatformPool();
        InitializeSpawnPosition();
        SpawnInitialPlatforms();
    }

    // 플레이어 위치를 추적하고 발판 생성 조건 확인
    void Update()
    {
        UpdatePlayerTracking();
        CheckSpawnCondition();
    }

    // 플레이어 참조 초기화
    private void InitializePlayerReference()
    {
        // 플레이어가 할당되지 않았다면 태그로 찾음
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // 오브젝트 풀 초기화
    private void InitializePlatformPool()
    {
        // 설정된 초기 풀 크기만큼 발판 생성
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject platform = Instantiate(SelectPlatform(), Vector3.zero, Quaternion.identity);
            platform.SetActive(false); // 비활성화 상태로 풀에 추가
            platformPool.Enqueue(platform);
        }
    }

    // 발판 생성 위치 초기화
    private void InitializeSpawnPosition()
    {
        highestPlayerY = player.position.y; // 플레이어의 초기 Y 위치
        nextSpawnY = transform.position.y; // 첫 발판 생성 Y 위치
    }

    // 초기 발판 생성
    private void SpawnInitialPlatforms()
    {
        // 3개의 초기 발판 생성
        for (int i = 0; i < 3; i++)
        {
            SpawnSinglePlatform();
            nextSpawnY += fixedYSpacing; // Y 위치 증가
        }
    }

    // 플레이어의 최고 Y 위치 갱신
    private void UpdatePlayerTracking()
    {
        float currentPlayerY = player.position.y;
        if (currentPlayerY > highestPlayerY)
            highestPlayerY = currentPlayerY;
    }

    // 발판 생성 조건 확인
    private void CheckSpawnCondition()
    {
        // 플레이어 Y 위치가 임계값에 도달하면 발판 생성
        if (highestPlayerY + spawnThreshold >= nextSpawnY)
        {
            SpawnSinglePlatform();
            nextSpawnY += fixedYSpacing; // 다음 생성 위치 갱신
        }
    }

    // 단일 발판 생성 및 설정
    private void SpawnSinglePlatform()
    {
        GameObject platform = GetPlatformFromPool(); // 풀에서 발판 가져오기
        SetupPlatformPosition(platform); // 위치 설정
        platform.SetActive(true); // 발판 활성화
        
        TrySpawnItemOnPlatform(platform); // 아이템 생성 시도
    }

    // 오브젝트 풀에서 발판 가져오기
    private GameObject GetPlatformFromPool()
    {
        if (platformPool.Count == 0)
        {
            return CreateNewPlatform(); // 풀이 비었으면 새 발판 생성
        }
        return platformPool.Dequeue(); // 풀에서 발판 꺼내기
    }

    // 새 발판 생성
    private GameObject CreateNewPlatform()
    {
        return Instantiate(SelectPlatform(), Vector3.zero, Quaternion.identity);
    }

    // 발판 위치 설정
    private void SetupPlatformPosition(GameObject platform)
    {
        float selectedX = SelectXPosition(); // 무작위 X 위치 선택
        platform.transform.position = new Vector3(selectedX, nextSpawnY, 0);
    }

    // 발판 위에 아이템 생성 시도
    private void TrySpawnItemOnPlatform(GameObject platform)
    {
        if (itemSpawner != null)
        {
            itemSpawner.TrySpawnItemOnPlatform(platform); // 아이템 생성기 호출
        }
    }

    // 발판을 오브젝트 풀로 반환
    public void ReturnPlatformToPool(GameObject platform)
    {
        platform.SetActive(false); // 발판 비활성화
        Score scoreComponent = platform.GetComponent<Score>(); // 점수 컴포넌트 가져오기
        if (scoreComponent != null)
        {
            scoreComponent.ResetPlatform(); // 점수 상태 초기화
        }
        platformPool.Enqueue(platform); // 풀에 발판 추가
    }

    // 확률 기반으로 발판 선택
    private GameObject SelectPlatform()
    {
        float totalChance = 0f;
        foreach (var p in platforms) totalChance += p.chance; // 총 확률 계산
        
        float randomValue = Random.value * totalChance; // 무작위 값 생성
        float cumulativeChance = 0f;

        // 확률에 따라 발판 선택
        foreach (var p in platforms)
        {
            cumulativeChance += p.chance;
            if (randomValue <= cumulativeChance)
                return p.platformPrefab;
        }
        return platforms[0].platformPrefab; // 기본값 반환
    }

    // 확률 기반으로 X 위치 선택
    private float SelectXPosition()
    {
        float totalChance = 0f;
        foreach (var pos in xPositions) totalChance += pos.chance; // 총 확률 계산
        
        float randomValue = Random.value * totalChance; // 무작위 값 생성
        float cumulativeChance = 0f;

        // 확률에 따라 X 위치 선택
        foreach (var pos in xPositions)
        {
            cumulativeChance += pos.chance;
            if (randomValue <= cumulativeChance)
                return pos.xPosition;
        }
        return xPositions[0].xPosition; // 기본값 반환
    }
}