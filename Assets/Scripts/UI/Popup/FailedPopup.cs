using Configs;
using Management.Roots;
using ResourceSystem;
using TMPro;
using UI.Popups;
using UnityEngine;
using Utils.Extensions;

public class FailedPopup : NotificationPopup
{
    [SerializeField] private TextMeshProUGUI _priceForAttempts;
    [SerializeField] private TextMeshProUGUI _levelText;

    private RewardedAdManager _rewardedAdManager;
    public void Initialize(GameplaySceneRoot gameplaySceneRoot, ResourceSystemService resourceService, RewardedAdManager rewardedAdManager, InterstitialAdvertisement interstitialAdvertisement)
    {
        _rewardedAdManager = rewardedAdManager;
        _interstitialAdvertisement = interstitialAdvertisement;
        base.Initialize(gameplaySceneRoot, resourceService, interstitialAdvertisement);
        _levelText.text = $"level {gameplaySceneRoot.LevelConfigData.LevelNumber}";
        _priceForAttempts.text = StringExtensions.GetAdaptedInt((uint)Remote.LevelConfig.LevelConfigData[0].HpCost);
        Show();
    }

    public void RewardedAd()
    {
        _rewardedAdManager.ShowAdForOneTry(AddOneTry);
    }

    public override void RestartButton()
    {
        _interstitialAdvertisement.ShowInterstitialAd();
        base.RestartButton();
    }

    public override void OkButton()
    {
        _interstitialAdvertisement.ShowInterstitialAd();
        base.OkButton();
    }

    public void BuyAttemptsButton()
    {
        if(_resourceService.GetResourceAmount(ResourceType.Gold) >= Remote.LevelConfig.LevelConfigData[0].HpCost)
        {
            _resourceService.SubtractResourceAmount(ResourceType.Gold, Remote.LevelConfig.LevelConfigData[0].HpCost);
            _gameplaySceneRoot.GameStateMachine.RechargeCannon();
            this.Hide();
        }
    }

    private void AddOneTry()
    {
        _gameplaySceneRoot.GameStateMachine.RechargeCannonOnce();
        this.Hide();
    }
}
