using UnityEngine;

public class BlinkingPlatform : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // 발판의 렌더러
    private Collider2D platformCollider;   // 발판의 충돌체

    public float visibleTime = 2f;   // 나타나는 시간 (초)
    public float invisibleTime = 1f; // 사라지는 시간 (초)
    private float timer;             // 타이머
    private bool isVisible = true;   // 현재 보이는 상태인지

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        platformCollider = GetComponent<Collider2D>();
        timer = visibleTime; // 처음엔 보이는 상태로 시작
    }

    void Update()
    {
        timer -= Time.deltaTime; // 시간 감소

        if (timer <= 0)
        {
            ToggleVisibility(); // 시간이 다 되면 상태 전환
        }
    }

    void ToggleVisibility()
    {
        isVisible = !isVisible; // 상태 반전

        // 보이기/숨기기
        spriteRenderer.enabled = isVisible;
        platformCollider.enabled = isVisible;

        // 타이머 리셋
        timer = isVisible ? visibleTime : invisibleTime;
    }
}