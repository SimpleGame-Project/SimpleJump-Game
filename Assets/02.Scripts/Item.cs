using UnityEngine;

public class Item : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            Debug.Log("아이템을 먹었습니다!");
        }
    }

    void Start()
{
    Invoke("SpawnItemsOnPlatforms", 0.1f); // 0.1초 지연
}
}