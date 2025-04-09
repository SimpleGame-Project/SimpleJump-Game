using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ItemChance
    {
        public GameObject itemPrefab;    // 아이템 프리팹
        public float chance;            // 아이템 생성 확률 (0 ~ 1)
    }

    public ItemChance[] items;
    public float itemYOffset = 0.5f;
    public string platformTag = "Platform";

    void Start()
    {
        SpawnItemsOnPlatforms();
    }

    void SpawnItemsOnPlatforms()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag(platformTag);
        if (platforms.Length == 0 || items.Length == 0) return;

        foreach (GameObject platform in platforms)
        {
            if (ShouldSpawnItem())
            {
                GameObject selectedItem = SelectRandomItem();
                Vector3 spawnPosition = platform.transform.position + new Vector3(0, itemYOffset, 0);
                Instantiate(selectedItem, spawnPosition, Quaternion.identity);
            }
        }
    }

    bool ShouldSpawnItem()
    {
        float totalChance = 0f;
        foreach (var item in items) totalChance += item.chance;
        return Random.value < totalChance;
    }

    GameObject SelectRandomItem()
    {
        float totalChance = 0f;
        foreach (var item in items) totalChance += item.chance;

        float randomValue = Random.value * totalChance;
        float cumulativeChance = 0f;

        foreach (var item in items)
        {
            cumulativeChance += item.chance;
            if (randomValue <= cumulativeChance)
            {
                return item.itemPrefab;
            }
        }
        return items[0].itemPrefab;
    }
}