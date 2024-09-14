using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Utilities;
using Zenject;

namespace UI
{
    public class StartDisplay : MonoBehaviour
    {
        [SerializeField] private Button _playButton;

        private PlayerDataProvider _playerDataProvider;
        private IAnalyticsLogService _analyticsLogService;

        [Inject]
        public void Inject(PlayerDataProvider playerDataProvider, IAnalyticsLogService analyticsLogService)
        {
            _playerDataProvider = playerDataProvider;
            _analyticsLogService = analyticsLogService;
        }

        private void OnEnable()
        {
            _playButton.onClick.AddListener(ToGameplayScene);
        }

        private void ToGameplayScene()
        {
            Debug.Log("GO!");
            _analyticsLogService.LogEvent("Tutorial_Play");

            if (_playerDataProvider.SaveData.IsTutorialCompleted)
            {
                SceneManager.LoadScene(ScenesMetadata.LevelsSceneName);
            }
            else
            {
                _analyticsLogService.LogEvent("Tutorial_First_Start");
                SceneManager.LoadScene(1);
            }
        }

        private void OnDisable()
        {
            _playButton.onClick.RemoveListener(ToGameplayScene);
        }

        public void Scene(int num)
        {
            SceneManager.LoadScene(num);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}