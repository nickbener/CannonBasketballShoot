using CodeBase;
using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using ResourceSystem;
using UI.Popups;
using UnityEngine;
using Zenject;

public class BonusPopUpView : MonoBehaviour
{
    [Header("PopUp")]
    [SerializeField] private Popup _boosterPopup;
    [SerializeField] private Popup _startBoosterPopup;
    [SerializeField] private Popup _learnBoosterPopup;
    [Header("Resources")]
    [SerializeField] private BonusResourceView _busterCannon;
    [SerializeField] private BonusResourceView _busterTime;
    [SerializeField] private BonusResourceView _busterDef;

    [SerializeField] private BonusResourceView _busterCannonLearn;
    [SerializeField] private BonusResourceView _busterTimeLearn;
    [SerializeField] private BonusResourceView _busterDefLearn;
    [Header("Configs")]
    [SerializeField] private ChooseBoosterConfig _chooseBoosterConfig;
    [SerializeField] private ChooseBoosterConfig _chooseStartBoosterConfig;
    [SerializeField] private ChooseBoosterConfig _chooseLearnBoosterConfig;

    private PlayerDataProvider _playerDataProvider;
    private IAnalyticsLogService _analyticsLogService;
    [Inject]
    private void Inject(PlayerDataProvider playerDataProvider, IAnalyticsLogService analyticsLogService)
    {
        _playerDataProvider = playerDataProvider;
        _analyticsLogService = analyticsLogService;
    }

    public void Initialize(
        ResourceSystemService resourceService,
        EventBusService eventBusService)
    {
        _busterCannon.Initialize(resourceService, eventBusService, _chooseBoosterConfig);
        _busterTime.Initialize(resourceService, eventBusService, _chooseBoosterConfig);
        _busterDef.Initialize(resourceService, eventBusService, _chooseBoosterConfig);

        _busterCannonLearn.Initialize(resourceService, eventBusService, _chooseLearnBoosterConfig);
        _busterTimeLearn.Initialize(resourceService, eventBusService, _chooseLearnBoosterConfig);
        _busterDefLearn.Initialize(resourceService, eventBusService, _chooseLearnBoosterConfig);
        _chooseBoosterConfig.Initialize(resourceService);
        _chooseLearnBoosterConfig.Initialize(resourceService);

        LevelSettings.BustSelected = _chooseBoosterConfig.BustSelected;
    }

    public void OpenBonusPupupButton(int level)
    {
        if (level == 1 || _playerDataProvider.SaveData.LevelsRecord.ContainsKey(level) || _playerDataProvider.SaveData.LevelsRecord.ContainsKey(level - 1))
        {
            //_chooseBoosterConfig.BustSelected[ResourceType.BustCannon] = false;
            //_chooseBoosterConfig.BustSelected[ResourceType.BustTime] = false;
            //_chooseBoosterConfig.BustSelected[ResourceType.BustDef] = false;
            LevelSettings.SelectedLevel = level;

            
            if (level == 4 && !_playerDataProvider.SaveData.IsTutorialBusterCompleted)
            {
                _analyticsLogService.LogEvent("Tutorial_Busters");
                _chooseLearnBoosterConfig.SetLevelText(level);
                _learnBoosterPopup.Show();
                LevelSettings.BustSelected = _chooseLearnBoosterConfig.BustSelected;
                _playerDataProvider.SaveData.IsTutorialBusterCompleted = true;
                _playerDataProvider.SaveData.DemandSave();
            }
            else
            {
                _chooseBoosterConfig.SetLevelText(level);
                _boosterPopup.Show();
                LevelSettings.BustSelected = _chooseBoosterConfig.BustSelected;
            }
        }
    }
    
    public void OpenStartBonusPupupButton(int level)
    {
        if (level == 1 || _playerDataProvider.SaveData.LevelsRecord.ContainsKey(level) || _playerDataProvider.SaveData.LevelsRecord.ContainsKey(level - 1))
        {
            LevelSettings.SelectedLevel = level;

            _chooseStartBoosterConfig.SetLevelText(level);
            _startBoosterPopup.Show();
        }
    }
}