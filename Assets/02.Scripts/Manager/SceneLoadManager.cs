using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : MonoBehaviour
{
    private static SceneLoadManager _instance;
    public static SceneLoadManager Instance
    {
        get
        {
            // 싱글톤 구현
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(SceneLoadManager)) as SceneLoadManager;

                if (_instance == null)
                    Debug.Log("인스턴스를 생성합니다");
            }
            return _instance;
        }
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

    public void LoadSceneAync(string sceneName, Action onLoaded = null)
    {
        StartCoroutine(LoadSceneAsyncCoroutine(sceneName, onLoaded));
    }

    private IEnumerator LoadSceneAsyncCoroutine(string sceneName, Action onLoaded)
    {
        // 씬 비동기 로딩
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        while(!asyncOperation.isDone) // 로딩 완료 전까지 대기
        {
            yield return null;
        }

        onLoaded?.Invoke();
    }
}