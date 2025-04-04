using UnityEngine;
using System.Collections.Generic;

public class PlatformSpawner : MonoBehaviour
{
    [System.Serializable]
    public class PlatformChance
    {
        public GameObject platformPrefab;
        public float chance;
    }

    [System.Serializable]
    public class XPositionChance
    {
        public float xPosition;
        public float chance;
    }

    public PlatformChance[] platforms;
    public XPositionChance[] xPositions;
    public float fixedYSpacing = 4f;
    public float spawnThreshold = 5f;
    public Transform player;
    public Camera mainCamera;
    public int initialPoolSize = 10; // 초기 풀 크기

    private float highestPlayerY;
    private float nextSpawnY;
    private Queue<GameObject> platformPool = new Queue<GameObject>();

    void Start()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;
        
        if (mainCamera == null)
            mainCamera = Camera.main;

        // 초기 발판 풀 생성
        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject platform = Instantiate(SelectPlatform(), Vector3.zero, Quaternion.identity);
            platform.SetActive(false);
            platformPool.Enqueue(platform);
        }

        highestPlayerY = player.position.y;
        nextSpawnY = transform.position.y;
        SpawnInitialPlatforms();
    }

    void Update()
    {
        if (player.position.y > highestPlayerY)
            highestPlayerY = player.position.y;

        if (highestPlayerY + spawnThreshold >= nextSpawnY)
        {
            SpawnPlatform();
            nextSpawnY += fixedYSpacing;
        }
    }

    void SpawnInitialPlatforms()
    {
        for (int i = 0; i < 3; i++)
        {
            SpawnPlatform();
            nextSpawnY += fixedYSpacing;
        }
    }

    void SpawnPlatform()
    {
        GameObject platform = GetPlatformFromPool();
        float selectedX = SelectXPosition();
        platform.transform.position = new Vector3(selectedX, nextSpawnY, 0);
        platform.SetActive(true);
    }

    GameObject GetPlatformFromPool()
    {
        if (platformPool.Count == 0)
        {
            // 풀이 비었을 때 새로 생성
            GameObject newPlatform = Instantiate(SelectPlatform(), Vector3.zero, Quaternion.identity);
            return newPlatform;
        }
        return platformPool.Dequeue();
    }

    public void ReturnPlatformToPool(GameObject platform)
    {
        platform.SetActive(false);
        platformPool.Enqueue(platform);
    }

    GameObject SelectPlatform()
    {
        float totalChance = 0f;
        foreach (var p in platforms) totalChance += p.chance;
        
        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var p in platforms)
        {
            cumulativeChance += p.chance;
            if (randomValue <= cumulativeChance)
                return p.platformPrefab;
        }
        return platforms[0].platformPrefab;
    }

    float SelectXPosition()
    {
        float totalChance = 0f;
        foreach (var pos in xPositions) totalChance += pos.chance;
        
        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var pos in xPositions)
        {
            cumulativeChance += pos.chance;
            if (randomValue <= cumulativeChance)
                return pos.xPosition;
        }
        return xPositions[0].xPosition;
    }
}