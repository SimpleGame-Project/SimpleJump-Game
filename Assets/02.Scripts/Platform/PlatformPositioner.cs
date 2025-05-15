using UnityEngine;

public class PlatformPositioner : MonoBehaviour
{
    public float minYSpacing = 1f;     // 최소 수직 간격
    public float maxYSpacing = 4f;     // 최대 수직 간격 (4로 고정)
    public float initialYSpacing = 4f; // 초반 5개 발판의 고정 Y 간격

    private float lastYPosition;

    public void Initialize(float startY)
    {
        lastYPosition = startY;
    }

    public float CalculateNextYPosition(bool isInitial)
    {
        float ySpacing;
        if (isInitial)
        {
            ySpacing = initialYSpacing; // 초반 발판은 고정 간격
        }
        else
        {
            ySpacing = Random.Range(minYSpacing, maxYSpacing); // 랜덤 간격
            Debug.Log($"랜덤 Y 간격: {ySpacing} (min: {minYSpacing}, max: {maxYSpacing})"); // 디버깅 로그
        }

        lastYPosition += ySpacing;
        return lastYPosition;
    }
}