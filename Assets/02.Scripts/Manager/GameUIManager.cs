using System;
using UnityEngine;
using UnityEngine.UI;

public class GameUIManager : MonoBehaviour
{
    private static GameUIManager _instance;
    public static GameUIManager Instance
    {
        get
        {
            // 싱글톤 구현
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameUIManager)) as GameUIManager;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
    }

    [Header("Game UI")]
    public Text scoreText;
    public Slider hpSlider;
    public Text hpText;

    [Header("EndPanel UI")]
    public GameObject endPanel;
    public Button endButton;
    public Text bestScore;
    public Text currScore;
    public Text goldText;

    private void Awake()
    {
        // 인스턴스가 존재하는데 이 오브젝트가 아니라면 파괴
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
    }

    private void Start()
    {
        endButton.onClick.AddListener(EndGame);
    }

    private void EndGame()
    {
        SceneLoadManager.Instance.LoadSceneAync("MainScene");
    }
    // 점수 텍스트 업데이트
    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"Floor: {score}";
    }

    // 체력 슬라이더 바 업데이트
    public void UpdateHpUI(int maxHp, int hp)
    {
        hpSlider.value = hp / maxHp;
        hpText.text = $"{hp} / {maxHp}";
    }

    // 게임 종료 시 결과창 활성화
    public void ActiveEndPanel()
    {
        endPanel.SetActive(true);

        bestScore.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        currScore.text = GameManager.Instance.GameScore.ToString();
        goldText.text = (PlayerPrefs.GetInt("BestScore", 0) * 100).ToString();
    }
}
