using Cysharp.Threading.Tasks;
using Game.Monetization.Ads;
using GoogleMobileAds.Api;
using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using Newtonsoft.Json.Linq;
using ResourceSystem;
using System;
using System.Collections;
using System.Net.Http;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class RewardedAdManager : MonoBehaviour
{
    private PlayerDataProvider _playerDataProvider;
    private ResourceSystemService _resourceSystemService;
    private IAdsService _adsService;
    private IAnalyticsLogService _analyticsLogService;

    private int _rewardGold;
    private Action _callbackAddTry;

    [Inject]
    public void Inject(PlayerDataProvider playerDataProvider, IAdsService adsService, IAnalyticsLogService analyticsLogService)
    {
        _playerDataProvider = playerDataProvider;
        _adsService = adsService;
        _analyticsLogService = analyticsLogService;
    }

    public void Initialize(ResourceSystemService resourceSystemService)
    {
        _resourceSystemService = resourceSystemService;
    }

    private void Start()
    {
        Initialize();

    }
    private async void Initialize()
    {

    }

    //BUTTONS
    public void ShowAdForMultiplyLevelReward(int gold)
    {
        _rewardGold = gold;
        _adsService.ShowRewarded(MultiplyGold);
    }
    public void ShowAdForOneTry(Action callback)
    {
        _callbackAddTry = callback;
        _adsService.ShowRewarded(AddTry);
    }
    //REWARD
    public async void MultiplyGold(Reward reward)
    {
        _resourceSystemService.AppendResourceAmount(ResourceType.Gold, _rewardGold);
    }
    public async void AddTry(Reward reward)
    {
        _callbackAddTry?.Invoke();
    }
    //OTHER
    private IEnumerator StartTimer(Button button, TextMeshProUGUI text, int minutes)
    {
        TimeSpan span = TimeSpan.FromMinutes(minutes);
        text.text = span.ToString("hh\\:mm");
        while (span.TotalMinutes > 0)
        {
            yield return new WaitForSecondsRealtime(60);
            span.Subtract(TimeSpan.FromMinutes(1));
            text.text = span.ToString("hh\\:mm");
        }
        button.interactable = true;
        text.gameObject.SetActive(false);
    }

    private async UniTask<DateTime> GetServerTime()
    {
        var client = new HttpClient();
        var responseBody = await client.GetStringAsync("http://worldtimeapi.org/api/timezone/Europe/Moscow");
        var response = JObject.Parse(responseBody);
        return DateTime.Parse(response["datetime"].ToString());
    }
}
