using UnityEngine;

public class GameOverTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.CompareTag("Player"))
            GameManager.Instance.GameOver();
    }
}
