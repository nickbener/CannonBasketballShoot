using Configs;
using Infrastructure.Providers;
using ResourceSystem;
using System;
using System.Collections;
using TMPro;
using UI.Popups;
using UnityEngine;
using Zenject;

public class ShopView : MonoBehaviour
{
    [SerializeField] private GameObject _tabGold;
    [SerializeField] private GameObject _tabBuster;
    [SerializeField] private GameObject _starterOffer;
    [SerializeField] private TextMeshProUGUI _starterOfferText;
    [SerializeField] private Popup _popup;

    private ResourceSystemService _resourceSystemService;
    private PlayerDataProvider _playerDataProvider;
    private Coroutine _coroutineStartTimer;

    [Inject]
    private void Inject(PlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
    }

    public void Initialize(ResourceSystemService resourceSystem)
    {
        _resourceSystemService = resourceSystem;
    }

    public void OpenShop()
    {
        _tabGold.SetActive(false);
        _tabBuster.SetActive(true);

        _popup.Show();
        StartTimerStarterOffer();
    }

    private void StartTimerStarterOffer()
    {
        _starterOffer.SetActive(false);
        if (_playerDataProvider.SaveData.IsStarterOfferTimerStarted)
        {
            int baseTime = Remote.ShopConfig.Shop_Stater_Pack_Time * 3600; // конвертируем часы в секунды
            TimeSpan dif = DateTime.Now - _playerDataProvider.SaveData.StarterOfferStartTime;
            if (dif.TotalSeconds < baseTime)
            {
                int remainTime = baseTime - (int)dif.TotalSeconds;
                _starterOffer.SetActive(true);
                if (_coroutineStartTimer != null)
                    StopCoroutine(_coroutineStartTimer);
                _coroutineStartTimer = StartCoroutine(StartTimer(remainTime));
            }
            else
            {
                _starterOfferText.text = "Time's Up";
            }
        }
    }

    public void BuyBusterTime()
    {
        if (_resourceSystemService.GetResourceAmount(ResourceType.Gold) < 50)
            return;
        _resourceSystemService.AppendResourceAmount(ResourceType.BustTime, 1);
        _resourceSystemService.SubtractResourceAmount(ResourceType.Gold, 50);
    }

    public void BuyBusterCannon()
    {
        if (_resourceSystemService.GetResourceAmount(ResourceType.Gold) < 50)
            return;
        _resourceSystemService.AppendResourceAmount(ResourceType.BustCannon, 1);
        _resourceSystemService.SubtractResourceAmount(ResourceType.Gold, 50);
    }
    public void BuyBusterDef()
    {
        if (_resourceSystemService.GetResourceAmount(ResourceType.Gold) < 50)
            return;
        _resourceSystemService.AppendResourceAmount(ResourceType.BustDef, 1);
        _resourceSystemService.SubtractResourceAmount(ResourceType.Gold, 50);
    }
    public void BuyBuster10()
    {
        if (_resourceSystemService.GetResourceAmount(ResourceType.Gold) < 1300)
            return;
        _resourceSystemService.AppendResourceAmount(ResourceType.BustDef, 10);
        _resourceSystemService.AppendResourceAmount(ResourceType.BustCannon, 10);
        _resourceSystemService.AppendResourceAmount(ResourceType.BustTime, 10);
        _resourceSystemService.SubtractResourceAmount(ResourceType.Gold, 1300);
    }
    public void BuyBuster100()
    {
        if (_resourceSystemService.GetResourceAmount(ResourceType.Gold) < 13000)
            return;
        _resourceSystemService.AppendResourceAmount(ResourceType.BustDef, 100);
        _resourceSystemService.AppendResourceAmount(ResourceType.BustCannon, 100);
        _resourceSystemService.AppendResourceAmount(ResourceType.BustTime, 100);
        _resourceSystemService.SubtractResourceAmount(ResourceType.Gold, 13000);
    }

    private IEnumerator StartTimer(int remainSeconds)
    {
        while (true)
        {
            TimeSpan time = TimeSpan.FromSeconds(remainSeconds);
            string timerText = string.Format("{0}d. {1:D2}h. {2:D2}m. {3:D2}s.", (int)time.TotalDays, time.Hours, time.Minutes, time.Seconds);
            _starterOfferText.text = timerText;
            yield return new WaitForSecondsRealtime(1);
            remainSeconds--;
        }
    }
}