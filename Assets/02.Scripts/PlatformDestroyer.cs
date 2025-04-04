using UnityEngine;

public class PlatformDestroyer : MonoBehaviour
{
    private Camera mainCamera;
    private float disableOffset = 2f;
    private PlatformSpawner spawner;

    void Start()
    {
        mainCamera = Camera.main;
        
        // [기존] Deprecated 방식 (Unity 2023.1 이전)
        // spawner = FindObjectOfType<PlatformSpawner>();
        
        // [수정] 새로운 방식 (Unity 2023.1 이후)
        spawner = FindFirstObjectByType<PlatformSpawner>();
        
        // 또는 (성능 우선시)
        // spawner = FindAnyObjectByType<PlatformSpawner>();
    }

    void Update()
    {
        float cameraBottomY = mainCamera.transform.position.y - mainCamera.orthographicSize - disableOffset;
        
        if (transform.position.y < cameraBottomY)
        {
            spawner.ReturnPlatformToPool(gameObject);
        }
    }
}