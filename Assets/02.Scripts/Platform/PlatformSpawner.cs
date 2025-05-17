using UnityEngine;

public class PlatformSpawner : MonoBehaviour
{
    public PlatformSelector platformSelector;
    public PlatformPositioner platformPositioner;
    public ObjectPooler objectPooler;
    public Transform platformsParent;
    public Transform playerTransform;
    private Camera mainCamera; // 메인 카메라 참조 추가

    public float spawnRate = 2f;
    private float nextSpawnTime;
    private int initialPlatformCount = 5;

    void Start()
    {
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

        if (playerTransform == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                playerTransform = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("플레이어 오브젝트를 찾을 수 없습니다. 'Player' 태그를 확인하세요.");
            }
        }

        // 메인 카메라 초기화
        mainCamera = Camera.main;

        platformPositioner.Initialize(transform.position.y, mainCamera); // 카메라 전달
        platformPositioner.playerTransform = playerTransform;
        objectPooler.Initialize(platformSelector, platformsParent);
        SpawnInitialPlatforms();
        nextSpawnTime = Time.time + spawnRate;

        PlatformCameraCuller culler = gameObject.GetComponent<PlatformCameraCuller>();
        if (culler == null)
        {
            culler = gameObject.AddComponent<PlatformCameraCuller>();
        }
        culler.platformsParent = platformsParent;
    }

    void Update()
    {
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
            SpawnPlatform(true);
        }
    }

    void SpawnRandomPlatform()
    {
        SpawnPlatform(false);
    }

    void SpawnPlatform(bool isInitial)
    {
        GameObject selectedPlatform = platformSelector.SelectRandomPlatform();
        float selectedX = platformSelector.SelectRandomXPosition();
        float newY = platformPositioner.CalculateNextYPosition(isInitial);
        Vector3 spawnPosition = new Vector3(selectedX, newY, 0);
        GameObject platform = objectPooler.GetPooledPlatform(selectedPlatform);
        if (platform != null)
        {
            platform.transform.position = spawnPosition;
            platform.transform.SetParent(platformsParent);
            platform.SetActive(true);
        }
        else
        {
            Debug.LogWarning("풀 크기 초과로 발판 생성이 중단되었습니다.");
        }
    }
}