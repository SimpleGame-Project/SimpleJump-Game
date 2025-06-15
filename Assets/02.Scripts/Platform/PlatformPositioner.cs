using UnityEngine;

// 발판 Y 위치를 고정 간격(ySpacing)으로 관리 (플레이어/카메라와 무관)
public class PlatformPositioner : MonoBehaviour
{
    [SerializeField] private float ySpacing = 4f; // 발판 간 Y 간격 (고정값)

    private float lastYPosition; // 마지막 발판의 Y 위치

    // 최초 시작 Y 위치 지정
    public void Initialize(float startY)
    {
        lastYPosition = startY;
    }

    // 다음 발판의 위치 반환 (항상 ySpacing만큼 위로)
    public Vector3 CalculateNextSpawnPosition(float x)
    {
        lastYPosition += ySpacing;
        return new Vector3(x, lastYPosition, 0);
    }
}
