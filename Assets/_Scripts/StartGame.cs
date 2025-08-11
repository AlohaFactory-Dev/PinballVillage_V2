using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace StartGameNamespace
{
    public class StartGame : MonoBehaviour
    {
        async void Start()
        {
            await GlobalConainer.Get<GameManager>().Init();

            // RootGameScene 씬 해제
            if (SceneManager.GetSceneByName("RootGameScene").isLoaded)
            {
                SceneManager.UnloadSceneAsync("RootGameScene");
            }
        }
    }
}