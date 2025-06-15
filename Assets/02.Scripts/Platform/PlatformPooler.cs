using UnityEngine;

// 플랫폼 풀 관리 매니저
public class PlatformPooler : MonoBehaviour
{
    [SerializeField] private JumpPlatform platformPrefab;      // 풀링할 발판 프리팹
    [SerializeField] private int poolSize = 10;                // 최대 풀 크기
    [SerializeField] private Transform platformsParent;        // 발판 부모 오브젝트

    private ObjectPool<JumpPlatform> pool;                     // 실제 풀

    void Awake()
    {
        pool = new ObjectPool<JumpPlatform>(platformPrefab, poolSize, platformsParent);
    }

    // 풀에서 발판 꺼내기
    public JumpPlatform GetPooledPlatform()
    {
        var platform = pool.Get();
        if (platform != null)
        {
            // onDeactivate 이벤트를 풀 반환으로 연결 (중복 방지)
            platform.onDeactivate.RemoveAllListeners();
            platform.onDeactivate.AddListener(() => pool.Return(platform));
        }
        return platform;
    }
}
