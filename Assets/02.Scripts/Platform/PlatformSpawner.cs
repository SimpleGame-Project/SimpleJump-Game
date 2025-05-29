using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public PlatformSelector platformSelector; // 발판 종류와 X 위치 선택 컴포넌트
    public PlatformPositioner platformPositioner; // Y 위치 계산 컴포넌트
    public ObjectPooler objectPooler; // 오브젝트 풀링 관리 컴포넌트
    public Transform platformsParent; // 발판 부모 오브젝트 (Hierarchy의 Platforms)
    public Transform playerTransform; // 플레이어 Transform (위치 기준)
    private Camera mainCamera; // 메인 카메라 참조 (뷰포트 계산용)

    public float spawnRate = 2f; // 발판 생성 주기 (초 단위)
    private float nextSpawnTime; // 다음 발판 생성 시점
    private int initialPlatformCount = 5; // 초반 생성 발판 수

    // 초기화: 발판 생성 준비
    void Start()
    {
        // Platforms 오브젝트 설정
        if (platformsParent == null)
        {
            GameObject platformsObj = GameObject.Find("Platforms"); // Hierarchy에서 Platforms 찾기
            if (platformsObj != null)
            {
                platformsParent = platformsObj.transform; // 있으면 참조
            }
            else
            {
                platformsObj = new GameObject("Platforms"); // 없으면 생성
                platformsParent = platformsObj.transform;
            }
        }

        // 플레이어 Transform 설정
        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player"); // Player 태그로 플레이어 찾기
            if (playerObj != null)
            {
                playerTransform = playerObj.transform; // 플레이어 Transform 설정
            }
            else
            {
                Debug.LogWarning("플레이어 오브젝트를 찾을 수 없습니다. 'Player' 태그를 확인하세요.");
            }
        }

        // 메인 카메라 초기화
        mainCamera = Camera.main; // 태그가 MainCamera인 카메라 참조

        // 발판 위치 및 풀 초기화
        platformPositioner.Initialize(transform.position.y, mainCamera); // 초기 Y 위치와 카메라 전달
        platformPositioner.playerTransform = playerTransform; // 플레이어 Transform 전달
        objectPooler.Initialize(platformSelector, platformsParent); // 오브젝트 풀 초기화
        SpawnInitialPlatforms(); // 초반 5개 발판 생성
        nextSpawnTime = Time.time + spawnRate; // 다음 생성 시간 설정

        // PlatformCameraCuller 컴포넌트 추가 및 설정
        PlatformCameraCuller culler = gameObject.GetComponent<PlatformCameraCuller>();
        if (culler == null)
        {
            culler = gameObject.AddComponent<PlatformCameraCuller>(); // 없으면 추가
        }
        culler.platformsParent = platformsParent; // Platforms 설정
    }

    // 매 프레임마다 발판 생성 주기 확인
    void Update()
    {
        if (Time.time >= nextSpawnTime) // 생성 주기 도달 시
        {
            SpawnRandomPlatform(); // 랜덤 발판 생성
            nextSpawnTime = Time.time + spawnRate; // 다음 생성 시간 갱신
        }
    }

    // 초반 5개 발판 생성
    void SpawnInitialPlatforms()
    {
        for (int i = 0; i < initialPlatformCount; i++)
        {
            SpawnPlatform(true); // 초반 발판 생성 (isInitial = true)
        }
    }

    // 랜덤 발판 생성
    void SpawnRandomPlatform()
    {
        SpawnPlatform(false); // 비초반 발판 생성 (isInitial = false)
    }

    // 발판 생성 메서드
    void SpawnPlatform(bool isInitial)
    {
        GameObject selectedPlatform = platformSelector.SelectRandomPlatform(); // 발판 종류 선택
        float selectedX = platformSelector.SelectRandomXPosition(); // X 위치 선택
        float newY = platformPositioner.CalculateNextYPosition(isInitial); // Y 위치 계산
        Vector3 spawnPosition = new Vector3(selectedX, newY, 0); // 생성 위치 설정
        GameObject platform = objectPooler.GetPooledPlatform(selectedPlatform); // 풀에서 발판 가져오기
        if (platform != null)
        {
            platform.transform.position = spawnPosition; // 위치 설정
            platform.transform.SetParent(platformsParent); // 부모 설정
            platform.SetActive(true); // 발판 활성화
        }
        else
        {
            Debug.LogWarning("풀 크기 초과로 발판 생성이 중단되었습니다.");
        }
    }
}