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

        // 랜덤으로 위로 또는 아래로 시작 결정 (50% 확률)
        moveUpFirst = Random.value > 0.5f;

        // 초기 위치를 시작 방향에 맞춰 설정
        float initialY = moveUpFirst ? minY : maxY;
        transform.position = new Vector3(startX, initialY, transform.position.z);
    }

    void Update()
    {
        // 이동 방향에 따라 계산
        float t = Mathf.PingPong(Time.time * moveSpeed, 1f); // 0 ~ 1 사이 왕복
        float newY;
        if (moveUpFirst)
        {
            newY = Mathf.Lerp(minY, maxY, t); // minY에서 maxY로 (위로 시작)
        }
        else
        {
            newY = Mathf.Lerp(maxY, minY, t); // maxY에서 minY로 (아래로 시작)
        }
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
            // 플랫폼이 활성화 상태인지 확인 후 부모 해제
            if (gameObject.activeInHierarchy)
            {
                StartCoroutine(SetParentNextFrame(collision.transform));
            }
        }
    }

    private IEnumerator SetParentNextFrame(Transform playerTransform)
    {
        yield return null; // 다음 프레임까지 대기
        if (playerTransform != null && gameObject.activeInHierarchy)
        {
            playerTransform.SetParent(null);
        }
    }

    // 플랫폼이 비활성화되거나 파괴될 때 호출
    void OnDisable()
    {
        // 모든 자식(플레이어 포함)을 부모에서 해제
        foreach (Transform child in transform)
        {
            if (child.CompareTag("Player"))
            {
                child.SetParent(null);
            }
        }
    }
}