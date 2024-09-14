using UnityEngine;
using Zenject;
using Game.Monetization.Ads;
using Infrastructure.Providers;

public class InterstitialAdvertisement : MonoBehaviour
{
    private IAdsService _adsService;
    private PlayerDataProvider _playerDataProvider;

    [Inject]
    public void Inject(IAdsService adsService, PlayerDataProvider playerDataProvider)
    {
        _adsService = adsService;
        _playerDataProvider = playerDataProvider;
    }

    public void Start()
    {
        //InvokeRepeating("ShowInterstitialAd", 60, 60);
    }

    public void ShowInterstitialAd()
    {
        //if (!_playerDataProvider.SaveData.UnconsumablePurchases.Contains("com.gm.imh.disableads"))
            _adsService.ShowInterstitial();
    }
}
