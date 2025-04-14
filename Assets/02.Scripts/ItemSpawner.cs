using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [System.Serializable]
    public class ItemChance
    {
        public GameObject itemPrefab;
        public float chance;
    }

    public ItemChance[] items;
    public float itemYOffset = 0.5f;
    public float spawnProbability = 0.3f;

    void Start()
    {
        SpawnItemsOnExistingPlatforms();
    }

    void SpawnItemsOnExistingPlatforms()
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms)
        {
            TrySpawnItemOnPlatform(platform);
        }
    }

    public void TrySpawnItemOnPlatform(GameObject platform)
    {
        if (ShouldSpawnItem())
        {
            SpawnItemOnPlatform(platform);
        }
    }

    void SpawnItemOnPlatform(GameObject platform)
    {
        GameObject selectedItem = SelectRandomItem();
        Vector3 spawnPosition = CalculateSpawnPosition(platform);
        
        GameObject item = Instantiate(selectedItem, spawnPosition, Quaternion.identity);
        item.transform.SetParent(platform.transform);
        
        SetupMovingItem(item, platform);
    }

    Vector3 CalculateSpawnPosition(GameObject platform)
    {
        Collider2D collider = platform.GetComponent<Collider2D>();
        if (collider != null)
        {
            return new Vector3(
                platform.transform.position.x,
                collider.bounds.max.y + itemYOffset,
                platform.transform.position.z
            );
        }
        return platform.transform.position + new Vector3(0, itemYOffset, 0);
    }

    void SetupMovingItem(GameObject item, GameObject platform)
    {
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Kinematic;
            rb.interpolation = RigidbodyInterpolation2D.Interpolate;
        }
        
        item.layer = LayerMask.NameToLayer("Items");
    }

    bool ShouldSpawnItem()
    {
        return Random.value < spawnProbability;
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