using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public PlatformSelector platformSelector; // 발판과 X 위치 선택
    public PlatformPositioner platformPositioner; // Y 위치 계산
    public ObjectPooler objectPooler; // 오브젝트 풀링 관리
    public Transform platformsParent; // 발판의 부모 오브젝트 (Platforms)

    public float spawnRate = 2f; // 발판 생성 주기
    private float nextSpawnTime;
    private int initialPlatformCount = 5; // 초반 생성 발판 수

    void Start()
    {
        // Platforms 오브젝트 찾기
        if (platformsParent == null)
        {
            GameObject platformsObj = GameObject.Find("Platforms");
            if (platformsObj != null)
            {
                platformsParent = platformsObj.transform;
            }
            else
            {
                platformsObj = new GameObject("Platforms");
                platformsParent = platformsObj.transform;
            }
        }

        // 초기 Y 위치 설정
        platformPositioner.Initialize(transform.position.y);

        // 오브젝트 풀 초기화
        objectPooler.Initialize(platformSelector, platformsParent);

        // 초반 5개 발판 생성
        SpawnInitialPlatforms();

        // 랜덤 생성을 위한 시간 설정
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        // 초반 5개 발판 생성 후, 랜덤 생성 시작
        if (Time.time >= nextSpawnTime)
        {
            SpawnRandomPlatform();
            nextSpawnTime = Time.time + spawnRate;
        }
    }

    void SpawnInitialPlatforms()
    {
        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform(true); // 초반 발판 생성
        }
    }

    void SpawnRandomPlatform()
    {
        SpawnPlatform(false); // 랜덤 발판 생성
    }

    void SpawnPlatform(bool isInitial)
    {
        // 발판 선택
        GameObject selectedPlatform = platformSelector.SelectRandomPlatform();

        // X 위치 선택
        float selectedX = platformSelector.SelectRandomXPosition();

        // Y 위치 계산
        float newY = platformPositioner.CalculateNextYPosition(isInitial);

        // 발판 생성 (오브젝트 풀에서 가져오기)
        Vector3 spawnPosition = new Vector3(selectedX, newY, 0);
        GameObject platform = objectPooler.GetPooledPlatform(selectedPlatform);
        if (platform != null)
        {
            platform.transform.position = spawnPosition;
            platform.transform.SetParent(platformsParent); // Platforms를 부모로 설정
            platform.SetActive(true);
        }
        else
        {
            Debug.LogWarning("풀 크기 초과로 발판 생성이 중단되었습니다.");
        }
    }
}