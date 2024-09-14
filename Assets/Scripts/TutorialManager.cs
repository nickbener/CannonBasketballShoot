using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;
using Zenject;

public class TutorialManager : MonoBehaviour
{
    [SerializeField] private GameObject _forceTutorial;
    private PlayerDataProvider _playerDataProvider;
    private IAnalyticsLogService _analyticsLogService;

    [Inject]
    public void Inject(
        PlayerDataProvider playerDataProvider,
        IAnalyticsLogService analyticsLogService
    )
    {
        _playerDataProvider = playerDataProvider;
        _analyticsLogService = analyticsLogService;
    }

    public void FirstShootCompleted()
    {
        _analyticsLogService.LogEvent("Tutorial_First_Shoot");

    }
    public void ScoreInfoCompleted()
    {
        _analyticsLogService.LogEvent("Tutorial_Score_Info");
        SceneManager.LoadScene(3);

    }
    public void HPInfoCompleted()
    {
        _analyticsLogService.LogEvent("Tutorial_HP_Info");

    }

    public void ForceSliderCompleted()
    {
        _analyticsLogService.LogEvent("Tutorial_Power_Shot");
        _forceTutorial.SetActive(false);
        _playerDataProvider.SaveData.IsTutorialForceCompleted = true;
        _playerDataProvider.SaveData.DemandSave();

    }

    public void TutorialCompleted()
    {
        if (string.IsNullOrEmpty(_playerDataProvider.SaveData.Nickname))
            ConstructNickName();
        _analyticsLogService.LogEvent("Tutorial_Finish");
        _playerDataProvider.SaveData.IsTutorialCompleted = true;
        PlayerPrefs.SetInt("tutorialCompleted", 1);
        Time.timeScale = 1;
        SceneManager.LoadScene(ScenesMetadata.LevelsSceneName);
    }

    private void ConstructNickName()
    {
        _playerDataProvider.SaveData.Nickname = $"user_{string.Format("{0:D5}", Random.Range(100, 99999))}";
        _playerDataProvider.SaveDataToFile();
    }
}
