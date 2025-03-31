using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float minX = -2f;    // 이동 시작 좌표 (왼쪽 끝)
    public float maxX = 2f;     // 이동 끝 좌표 (오른쪽 끝)
    public float moveSpeed = 2f; // 이동 속도
    private float startY;       // 초기 Y 위치 (고정)

    void Start()
    {
        startY = transform.position.y; // Y 위치는 고정
        // 시작 위치를 minX와 maxX 사이에 맞추려면 초기 위치 조정 가능
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, minX, maxX), startY, transform.position.z);
    }

    void Update()
    {
        // PingPong으로 minX와 maxX 사이를 왕복
        float newX = minX + Mathf.PingPong(Time.time * moveSpeed, maxX - minX);
        transform.position = new Vector3(newX, startY, transform.position.z);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(transform); // 플레이어를 발판의 자식으로
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collision.transform.SetParent(null); // 부모 관계 해제
        }
    }
}