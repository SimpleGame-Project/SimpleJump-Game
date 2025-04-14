using UnityEngine;
using UnityEngine.UI;

public class GameStartButton : MonoBehaviour
{
    private Button startBtn;

    void Awake()
    {
        startBtn = GetComponent<Button>();
        startBtn.onClick.AddListener(StartGame);
    }

    void StartGame()
    {
        GameManager.Instance.StartGame();
    }
}
