using UnityEngine;

public class PlatformCameraCuller : MonoBehaviour
{
    public Transform platformsParent; // 발판 부모 오브젝트 (Hierarchy의 Platforms 참조)
    public Camera mainCamera; // 메인 카메라 참조 (뷰포트 계산용)

    // 초기화: 메인 카메라 설정
    void Start()
    {
        // 메인 카메라 초기화
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // 씬의 메인 카메라 자동 할당 (태그: MainCamera)
        }
    }

    // 매 프레임마다 발판 상태 점검
    void Update()
    {
        CheckPlatformsOutOfCamera(); // 카메라 밖 발판 비활성화 로직 호출
    }

    // 카메라 밖 발판을 비활성화하는 메서드
    void CheckPlatformsOutOfCamera()
    {
        if (mainCamera == null || platformsParent == null) return; // 카메라 또는 부모가 없으면 종료

        // Platforms 아래 모든 자식 발판 순회
        foreach (Transform platform in platformsParent)
        {
            // 발판의 월드 좌표를 뷰포트 좌표로 변환 (0~1 범위)
            Vector3 viewportPoint = mainCamera.WorldToViewportPoint(platform.position);
            // 뷰포트 좌표가 -0.1f ~ 1.1f 밖에 있으면 카메라 밖
            if (viewportPoint.x < -0.1f || viewportPoint.x > 1.1f || // X축 밖
                viewportPoint.y < -0.1f || viewportPoint.y > 1.1f)   // Y축 밖
            {
                platform.gameObject.SetActive(false); // 카메라 밖이면 비활성화
            }
        }
    }
}