using System;
using Jang;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{
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
    public bool[] isUpgrade = {false, false, false};
    public PlayerController player;
    void Start()
    {
        Gold = PlayerPrefs.GetInt("Gold", 0);
    }

    public void StartGame()
    {
        SFXManager.Instance.PlayClickSound();
        
        SceneLoadManager.Instance.LoadSceneAync("StoryMode", () =>
        {
            GameScore = 0;

            player = GameObject.FindWithTag("Player").GetComponent<PlayerController>();

            // 구입한 업그레이드 적용
            if (isUpgrade[0]) { player.Attack += 50; isUpgrade[0] = false; }
            if (isUpgrade[1]) { player.Shield += 3; isUpgrade[1] = false; }
            if (isUpgrade[2]) { player._jumpForce += 5f; isUpgrade[2] = false; }

            UIManager.Instance.InitHpShieldUI(player.MaxHp);
        });
    }

    public void RewardGold(int rewardGold)
    {
        Gold += rewardGold;
        PlayerPrefs.SetInt("Gold", Gold);
    }
}
