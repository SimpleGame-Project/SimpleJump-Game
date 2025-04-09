using UnityEngine;
using System.Collections;

public class VerticalMovingPlatform : MonoBehaviour
{
    public float moveRange = 3f;     // 이동 범위
    public float moveSpeed = 1f;     // 이동 속도
    private float minY;             // 아래 끝 좌표
    private float maxY;             // 위 끝 좌표
    private float startX;           // 초기 X 위치
    private bool moveUpFirst;       // 처음에 위로 갈지 여부

    void Start()
    {
        startX = transform.position.x;
        float startY = transform.position.y;
        minY = startY - (moveRange / 2f);
        maxY = startY + (moveRange / 2f);

        moveUpFirst = Random.value > 0.5f;
        float initialY = moveUpFirst ? minY : maxY;
        transform.position = new Vector3(startX, initialY, transform.position.z);
    }

    void Update()
    {
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f);
        float newY = moveUpFirst ? Mathf.Lerp(minY, maxY, t) : Mathf.Lerp(maxY, minY, t);
        transform.position = new Vector3(startX, newY, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform);
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // 플랫폼이 활성화 상태일 때만 부모 해제
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(SetParentNextFrame(collision.transform));
            }
        }
    }

    private IEnumerator SetParentNextFrame(Transform playerTransform)
    {
        yield return null; // 다음 프레임 대기
        if (playerTransform != null && gameObject.activeInHierarchy)
        {
            playerTransform.SetParent(null);
        }
    }

    // 플랫폼이 비활성화될 때 호출 (안전하게 처리)
    void OnDisable()
    {
        // 플랫폼이 비활성화되기 전에 자식 확인
        for (int i = transform.childCount - 1; i >= 0; i--)
        {
            Transform child = transform.GetChild(i);
            if (child.CompareTag("Player") && child != null)
            {
                // 비활성화 중이라 바로 SetParent를 호출하지 않고, 플레이어가 스스로 처리하도록 남겨둠
                child.SetParent(null); // 안전하게 해제되도록 수정
            }
        }
    }
}