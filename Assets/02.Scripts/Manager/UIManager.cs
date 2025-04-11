using System;
using Jang;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            // 싱글톤 구현
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(UIManager)) as UIManager;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
    }

    [Header("Game UI")]
    [SerializeField] Text scoreText;
    [SerializeField] Transform hpContainer;
    private Transform[] hearts;
    [SerializeField] GameObject heart_Prefab;
    [SerializeField] Transform[] shields;

    [Header("EndPanel UI")]
    [SerializeField] GameObject endPanel;
    [SerializeField] Button endButton;
    [SerializeField] Text bestScore;
    [SerializeField] Text currScore;
    [SerializeField] Text goldText;

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

    public void InitHpShieldUI(int maxHp)
    {
        hearts = new Transform[maxHp];

        for (int i = 0; i < maxHp; i++)
        {
            hearts[i] = Instantiate(heart_Prefab, hpContainer).transform;
        }

        for(int i = 0; i < GameManager.Instance.player.Shield; i++)
        {
            shields[i].GetChild(0).gameObject.SetActive(true);
        }
    }

    // 점수 텍스트 업데이트
    public void UpdateScoreUI(int score)
    {
        scoreText.text = $"Floor: {score}";
    }

    // 체력 UI 업데이트
    public void UpdateHpUI(int maxHp, int hp)
    {
        GameObject obj = hearts[Math.Max(0, maxHp - hp)].GetChild(0).gameObject;
        obj.SetActive(!obj.activeSelf);
    }

    public void UpdateShieldUI(int shield)
    {   
        GameObject obj = shields[Math.Max(0, 3 - shield)].GetChild(0).gameObject;
        obj.SetActive(!obj.activeSelf);
    }

    // 게임 종료 시 결과창 활성화
    public void ActiveEndPanel()
    {
        // 최고 기록 갱신
        if (GameManager.Instance.GameScore > PlayerPrefs.GetInt("BestScore", 0)) PlayerPrefs.SetInt("BestScore", GameManager.Instance.GameScore);

        endPanel.SetActive(true);

        bestScore.text = PlayerPrefs.GetInt("BestScore", 0).ToString();
        currScore.text = GameManager.Instance.GameScore.ToString();
        goldText.text = $"{GameManager.Instance.GameScore * 100}";
    }

    private void EndGame()
    {
        SFXManager.Instance.PlayClickSound();
        
        // MainScene 로딩이 완료되면 보상Gold 지급
        SceneLoadManager.Instance.LoadSceneAync("MainScene", () =>
        GameManager.Instance.RewardGold(GameManager.Instance.GameScore * 100));
    }
}
