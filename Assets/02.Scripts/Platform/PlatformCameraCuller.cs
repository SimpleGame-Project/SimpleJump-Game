using UnityEngine;

// 플랫폼이 카메라 뷰포트 밖에 있으면 비활성화 (일정 주기마다)
public class PlatformCameraCuller : MonoBehaviour
{
    [SerializeField] private Transform platformsParent;    // Platforms 오브젝트
    [SerializeField] private Camera mainCamera;            // 메인 카메라
    [SerializeField] private float cullInterval = 0.2f;    // 체크 주기(초)
    private float timer;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer < cullInterval) return;
        timer = 0f;

        foreach (Transform platform in platformsParent)
        {
            Vector3 vp = mainCamera.WorldToViewportPoint(platform.position);
            bool inView = vp.x >= -0.1f && vp.x <= 1.1f && vp.y >= -0.1f && vp.y <= 1.1f;
            // 상태가 바뀔 때만 SetActive 호출 (최적화)
            if (platform.gameObject.activeSelf != inView)
                platform.gameObject.SetActive(inView);
        }
    }
}
