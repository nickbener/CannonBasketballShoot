using Factories;
using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using Management.Roots;
using ResourceSystem;
using UnityEngine;
using Zenject;

public class PopupManager : MonoBehaviour
{
    [SerializeField] private FailedPopup _failedPopup;
    [SerializeField] private CompletePopup _completePopup;
    [SerializeField] private WordPopup _goalPopup;
    [SerializeField] private WordPopup _excellentPopup;
    [SerializeField] private WordPopup _superPopup;
    [SerializeField] private StarterOfferPopup _starterOffer;
    [SerializeField] private LoseOfferPopup _loseOffer;
    [SerializeField] private VisualEffectFactory _vfxFactory;

    private GameplaySceneRoot _gameplaySceneRoot;
    private ResourceSystemService _resourceService;
    private PlayerDataProvider _playerDataProvider;
    private RewardedAdManager _rewardedAdManager;
    private PurchaseListener _purchaseListener;
    private InterstitialAdvertisement _interstitialAdvertisement;
    private IAnalyticsLogService _analyticsLogService;

    [Inject]
    public void Inject(PlayerDataProvider playerDataProvider, IAnalyticsLogService analyticsLogService)
    {
        _playerDataProvider = playerDataProvider;
        _analyticsLogService = analyticsLogService;
    }

    public void Initialize(GameplaySceneRoot gameplaySceneRoot, ResourceSystemService resourceService, RewardedAdManager rewardedAdManager, InterstitialAdvertisement interstitialAdvertisement, PurchaseListener purchaseListener)
    {
        _gameplaySceneRoot = gameplaySceneRoot;
        _resourceService = resourceService;
        _rewardedAdManager = rewardedAdManager;
        _interstitialAdvertisement = interstitialAdvertisement;
        _purchaseListener = purchaseListener;

        _gameplaySceneRoot.GameStateMachine.LevelFailed += OnLevelFailed;
        _gameplaySceneRoot.GameStateMachine.LevelCompleted += OnLevelCompleted;
        _gameplaySceneRoot.GameStateMachine.GoalScored += OnGoalScored;
        _purchaseListener.PurchaseCompleted += OnPurchaseCompleted;
    }

    private void OnDestroy()
    {
        _gameplaySceneRoot.GameStateMachine.LevelFailed -= OnLevelFailed;
        _gameplaySceneRoot.GameStateMachine.LevelCompleted -= OnLevelCompleted;
        _gameplaySceneRoot.GameStateMachine.GoalScored -= OnGoalScored;
        _purchaseListener.PurchaseCompleted -= OnPurchaseCompleted;
    }

    private void OnPurchaseCompleted(string id)
    {
        switch (id)
        {
            case "com.gamezmonster.cannonbasketball.starteroffer":
                _starterOffer.Hide();
                break;
            case "com.gamezmonster.cannonbasketball.loseoffer0001":
                _loseOffer.Hide();
                break;
            default:
                break;
        }
    }

    private void OnGoalScored(int val)
    {
        if (val > 2)
            //_excellentPopup.ShowWordPopup();
            _vfxFactory.CreateExclamation(new Vector2(0.0f, 1.0f), "EXCELLENT");
        else if (val > 1)
            //_superPopup.ShowWordPopup();
            _vfxFactory.CreateExclamation(new Vector2(0.0f, 1.0f), "SUPER");
        else
            //_goalPopup.ShowWordPopup();
            _vfxFactory.CreateExclamation(new Vector2(0.0f, 1.0f), "GOAL");
    }

    private void OnLevelCompleted(int amountStars)
    {
        _analyticsLogService.LogEvent($"Level_Finish_{string.Format("{0:D4}", LevelSettings.SelectedLevel)}");
        _completePopup.Initialize(_gameplaySceneRoot, _resourceService, amountStars, _rewardedAdManager, _interstitialAdvertisement);
        _playerDataProvider.SaveDataToFile();
        if (LevelSettings.SelectedLevel == 5 && !_playerDataProvider.SaveData.UnconsumablePurchases.Contains("com.gamezmonster.cannonbasketball.starteroffer"))
            _starterOffer.ShowOffer();
    }

    private void OnLevelFailed()
    {
        _failedPopup.Initialize(_gameplaySceneRoot, _resourceService, _rewardedAdManager, _interstitialAdvertisement);
        if (LevelSettings.SelectedLevel > 3)
            _loseOffer.ShowOffer();
    }
}
