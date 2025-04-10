using System;
using Jang;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(GameManager)) as GameManager;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
    }
    private int _gameScore;
    private int _gold;
    public int Gold
    {
        set
        {
            _gold = Math.Max(0, value);

            if (MainUIManager.Instance != null)
                MainUIManager.Instance.UpdateGoldUI(_gold);
        }

        get => _gold;
    }
    public int GameScore
    {
        set
        {
            _gameScore = Math.Max(0, value);

            if (UIManager.Instance != null) ;
            UIManager.Instance.UpdateScoreUI(_gameScore);
        }

        get => _gameScore;
    }
    public bool isAttackUpgrade = false;
    public bool isShieldUpgrade = false;
    public bool isJumpUpgrade = false;
    private void Awake()
    {
        // 인스턴스가 존재하는데 이 오브젝트가 아니라면 파괴
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this;
        // 씬 로드시에도 파괴되지않음 
        DontDestroyOnLoad(gameObject);

        Gold = PlayerPrefs.GetInt("Gold", 0);
    }

    public void StartGame()
    {
        SceneLoadManager.Instance.LoadSceneAync("StoryMode", () =>
        {
            GameScore = 0;

            PlayerController player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            // 구입한 업그레이드 적용
            if (isAttackUpgrade) { player.Attack += 50; isAttackUpgrade = false; }
            if (isShieldUpgrade) { player.Shield += 3; isShieldUpgrade = false; }
            if (isJumpUpgrade) { player._jumpForce += 5f; isJumpUpgrade = false; }
        });
    }

    public void RewardGold(int rewardGold)
    {
        Gold += rewardGold;
    }
}
