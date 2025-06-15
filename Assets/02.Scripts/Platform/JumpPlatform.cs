using UnityEngine;
using UnityEngine.Events;

// 풀링 대상 플랫폼 오브젝트
public class JumpPlatform : MonoBehaviour, IPoolable
{
    public UnityEvent onDeactivate; // 비활성화(풀 반환) 이벤트

    // 풀에서 꺼낼 때 호출
    public void OnGetFromPool()
    {
        // 필요 시 상태 초기화
    }

    // 풀로 반환될 때 호출
    public void OnReturnToPool()
    {
        // 필요 시 상태 초기화
    }

    // 카메라 밖으로 나가면 풀 반환 이벤트 호출
    private void OnBecameInvisible()
    {
        onDeactivate?.Invoke();
    }
}
