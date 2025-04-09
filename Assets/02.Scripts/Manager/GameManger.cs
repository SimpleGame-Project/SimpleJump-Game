using System;
using UnityEngine;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            // 싱글톤 구현
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
        }

        get => _gold;
    }
    public int GameScore
    {
        set
        {
            _gameScore = Math.Max(0, value);

            if(GameUIManager.Instance != null);
                GameUIManager.Instance.UpdateScoreUI(_gameScore);
        }

        get => _gameScore;
    }

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
    }

    public void StartGame()
    {
        SceneLoadManager.Instance.LoadSceneAync("StoryMode", () =>
        {
            GameScore = 0;
        });
    }

    public void GameOver()
    {
        if(GameScore > PlayerPrefs.GetInt("BestScore", 0)) PlayerPrefs.SetInt("BestScore", GameScore);
        GameUIManager.Instance.ActiveEndPanel();
    }
}
