using Management.Roots;
using ResourceSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

public class GameplayUI : MonoBehaviour
{
    private GameplaySceneRoot _gameplaySceneRoot;

    public void Initialize(GameplaySceneRoot gameplaySceneRoot)
    {
        _gameplaySceneRoot = gameplaySceneRoot;
    }

    public void ExitButton()
    {
        _gameplaySceneRoot.Dispose();
        SceneManager.LoadScene(ScenesMetadata.LevelsSceneName);
    }
}
