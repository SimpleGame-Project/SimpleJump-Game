using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadManager : Singleton<SceneLoadManager>
{
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

        // 완료 신호
        onLoaded?.Invoke();
    }
}