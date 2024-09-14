using Infrastructure.Providers;
using TMPro;
using UI.Popups;
using UnityEngine;
using Zenject;

public class OfferPopup : Popup
{
    [SerializeField] protected TextMeshProUGUI coins_count;
    [SerializeField] protected TextMeshProUGUI cannonBonus_count;
    [SerializeField] protected TextMeshProUGUI timerBonus_count;
    [SerializeField] protected TextMeshProUGUI shieldBonus_count;
    [SerializeField] protected TextMeshProUGUI cost;
    [SerializeField] protected TextMeshProUGUI timerOClock;
    [SerializeField] protected bool useTimer;

    protected PlayerDataProvider _playerDataProvider;
    protected int startTimeHours; // Начальное время в часах

    [Inject]
    private void Inject(PlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
    }

    public void ShowOffer()
    {
        if (useTimer)
            StartTimer();
        Show();
    }

    protected virtual void StartTimer()
    {

    }
}