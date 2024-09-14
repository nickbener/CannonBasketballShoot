using Configs;
using Management.Roots;
using ResourceSystem;
using TMPro;
using UI.Popups;
using UnityEngine;
using UnityEngine.UI;
using Utils.Extensions;

public class CompletePopup : NotificationPopup
{
    [SerializeField] private GameObject _rewardedButton;
    [SerializeField] private TextMeshProUGUI _amountGold;
    [SerializeField] private TextMeshProUGUI _levelText;
    [Space]
    [SerializeField] private Image[] _stars;
    [SerializeField] private Sprite[] _starsEmptySprite;
    [SerializeField] private Sprite[] _starsYellowSprite;

    private RewardedAdManager _rewardedAdManager;
    private InterstitialAdvertisement _interstitialAdvertisement;
    private int _gold;

    public void Initialize(GameplaySceneRoot gameplaySceneRoot, ResourceSystemService resourceService, int amountStars, RewardedAdManager rewardedAdManager, InterstitialAdvertisement interstitialAdvertisement)
    {
        _rewardedAdManager = rewardedAdManager;
        _interstitialAdvertisement = interstitialAdvertisement;
        base.Initialize(gameplaySceneRoot, resourceService, interstitialAdvertisement);
        _levelText.text = $"level {gameplaySceneRoot.LevelConfigData.LevelNumber}";
        _gold = CalculateReward(amountStars);
        for (int i = 0; i < 3; i++)
        {
            _stars[i].sprite = _starsEmptySprite[i];
        }
        for (int i = 0; i < amountStars; i++)
        {
            _stars[i].sprite = _starsYellowSprite[i];
        }
        Show();
        _gameplaySceneRoot.ResourceService.AppendResourceAmount(ResourceType.Star, amountStars);
        _interstitialAdvertisement.ShowInterstitialAd();
    }

    public void RewardedAd()
    {
        _rewardedButton.SetActive(false);
        _rewardedAdManager.ShowAdForMultiplyLevelReward(_gold);
    }

    private int CalculateReward(int amountStars)
    {
        int amount = Remote.LevelConfig.LevelConfigData[0].LevelValue 
            + Remote.LevelConfig.LevelConfigData[0].LevelHPValue * _gameplaySceneRoot.GameStateMachine.Cannon.CurrentBalls
            + Remote.LevelConfig.LevelConfigData[0].LevelStarValue * amountStars;
        _amountGold.text = StringExtensions.GetAdaptedInt((uint)amount);

        _gameplaySceneRoot.ResourceService.AppendResourceAmount(ResourceType.Gold, amount);
        return amount;
    }
}
