using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] private int points = 10; // 발판당 점수
    private bool hasBeenStepped = false; // 중복 점수 방지

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !hasBeenStepped)
        {
            hasBeenStepped = true;
            GameManager.instance.AddScore(points);
        }
    }

    // PlatformSpawner에서 재활용 시 초기화
    public void ResetPlatform()
    {
        hasBeenStepped = false;
    }
}