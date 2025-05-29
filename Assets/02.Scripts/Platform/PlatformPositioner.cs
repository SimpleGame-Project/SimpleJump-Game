using UnityEngine;

public class PlatformPositioner : MonoBehaviour
{
    public float minYSpacing = 1f;     // 최소 수직 간격 (비초반 발판 간격)
    public float maxYSpacing = 4f;     // 최대 수직 간격 (비초반 발판 간격)
    public float initialYSpacing = 4f; // 초반 5개 발판의 고정 Y 간격
    public Transform playerTransform;  // 플레이어 Transform (선택적, 위치 기준)
    public Camera mainCamera;          // 메인 카메라 참조 (뷰포트 계산용)

    private float lastYPosition; // 마지막 발판의 Y 위치 저장

    // 초기화: 시작 Y 위치와 카메라 설정
    public void Initialize(float startY, Camera camera)
    {
        mainCamera = camera; // 카메라 설정
        if (mainCamera != null)
        {
            // 카메라 뷰포트 상단 기준으로 초기 Y 위치 설정
            Vector3 viewportTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));
            lastYPosition = viewportTop.y - (initialYSpacing * 2); // 초반 5개가 화면 상단 아래로 시작
        }
        else
        {
            lastYPosition = startY; // 카메라 없으면 기본값 사용
        }
    }

    // 다음 발판의 Y 위치 계산
    public float CalculateNextYPosition(bool isInitial)
    {
        float ySpacing;
        if (isInitial)
        {
            ySpacing = initialYSpacing; // 초반 발판은 고정 간격 사용
        }
        else
        {
            // 카메라 뷰포트 상단 기준으로 Y 위치 계산
            if (mainCamera != null)
            {
                // 뷰포트 상단(1)과 하단(0)을 월드 좌표로 변환
                Vector3 viewportTop = mainCamera.ViewportToWorldPoint(new Vector3(0, 1, mainCamera.nearClipPlane));
                Vector3 viewportBottom = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));

                // lastYPosition을 카메라 뷰포트 내로 조정
                lastYPosition = Mathf.Max(lastYPosition, viewportBottom.y);
                if (lastYPosition < viewportTop.y)
                {
                    lastYPosition = viewportTop.y; // 카메라 상단에서 시작
                }
            }
            ySpacing = Random.Range(minYSpacing, maxYSpacing); // 비초반 발판은 랜덤 간격
        }

        lastYPosition += ySpacing; // 다음 위치 계산
        return lastYPosition; // 계산된 Y 위치 반환
    }
}