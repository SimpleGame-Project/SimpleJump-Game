using UnityEngine;

public class Playermove : MonoBehaviour
{
    public float maxSpeed = 5f; // 이동속도 제한
    public float jumpForce = 30f; // 점프힘 (값을 늘림)
    private Rigidbody2D rigid;
    private bool isGrounded; // 땅에 닿았는지 확인

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        rigid.constraints = RigidbodyConstraints2D.FreezeRotation; // 회전 고정
    }

    void Update()
    {
        // 점프 입력
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigid.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            isGrounded = false;
            Debug.Log("점프 입력 감지!");
        }

        // 아래로 통과 입력
        if (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S))
        {
            IgnorePlatformCollision(true); // 플랫폼 통과 활성화
        }
        else
        {
            IgnorePlatformCollision(false); // 플랫폼 통과 비활성화
        }

        CheckGrounded(); // 착지 상태 확인
    }

    void FixedUpdate()
    {
        // 좌우 이동 입력
        float h = Input.GetAxisRaw("Horizontal");
        Vector2 targetVelocity = new Vector2(h * maxSpeed, rigid.linearVelocity.y);
        rigid.linearVelocity = targetVelocity;

        // 속도 제한
        if (rigid.linearVelocity.x > maxSpeed)
            rigid.linearVelocity = new Vector2(maxSpeed, rigid.linearVelocity.y);
        else if (rigid.linearVelocity.x < -maxSpeed)
            rigid.linearVelocity = new Vector2(-maxSpeed, rigid.linearVelocity.y);
    }

    // 착지 상태 확인
    private void CheckGrounded()
{
    Vector2 checkPosition = (Vector2)transform.position + Vector2.down * 0.5f; // 발밑 체크 (위치 조정)
    Debug.DrawRay(checkPosition, Vector2.down * 0.5f, Color.red); // 시각적 확인
    Collider2D hit = Physics2D.OverlapCircle(checkPosition, 0.5f, LayerMask.GetMask("Platform")); // 반경 조정
    if (hit != null && rigid.linearVelocity.y <= 0) // 내려오는 중일 때 착지
    {
        isGrounded = true;
        Debug.Log("착지 상태: true");
    }
    else if (rigid.linearVelocity.y > 0) // 올라가는 중이면 공중
    {
        isGrounded = false;
        Debug.Log("착지 상태: false");
    }
}

    // 플랫폼 충돌 무시
    private void IgnorePlatformCollision(bool ignore)
    {
        GameObject[] platforms = GameObject.FindGameObjectsWithTag("Platform");
        foreach (GameObject platform in platforms)
        {
            Collider2D platformCollider = platform.GetComponent<Collider2D>();
            Collider2D playerCollider = GetComponent<Collider2D>();
            if (platformCollider != null && playerCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, platformCollider, ignore);
            }
        }
    }
}