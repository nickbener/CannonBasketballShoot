using Configs;
using System;
using UnityEngine;

public class StarterOfferPopup : OfferPopup
{
    private bool timerStarted;
    private float remainTime;
    new private void Awake()
    {
        base.Awake();
        coins_count.text = Remote.ShopConfig.Shop_Starter_Pack_Coins.ToString();
        cannonBonus_count.text = Remote.ShopConfig.Shop_Starter_Pack_Control.ToString();
        timerBonus_count.text = Remote.ShopConfig.Shop_Starter_Pack_Bullet_Time.ToString();
        shieldBonus_count.text = Remote.ShopConfig.Shop_Starter_Pack_Shields.ToString();
        cost.text = Remote.ShopConfig.Shop_Starter_Pack_Cost.ToString() + "$";
    }

    protected override void StartTimer()
    {
        if (!_playerDataProvider.SaveData.IsStarterOfferTimerStarted)
        {
            _playerDataProvider.SaveData.IsStarterOfferTimerStarted = true;
            _playerDataProvider.SaveData.StarterOfferStartTime = DateTime.Now;
        }

        int baseTime = Remote.ShopConfig.Shop_Stater_Pack_Time * 3600; // конвертируем часы в секунды
        TimeSpan dif = DateTime.Now - _playerDataProvider.SaveData.StarterOfferStartTime;
        if(dif.TotalSeconds < baseTime)
        {
            remainTime = baseTime - (int)dif.TotalSeconds;
            timerStarted = true;
        }
        else
        {
            timerOClock.text = "Time's Up";
            timerStarted = false;
        }
        Time.timeScale = 1;
    }

    private void Update()
    {
        // Проверяем, было ли уже сохранено время начала отсчета
        if (!timerStarted)
            return;

        remainTime -= Time.deltaTime;

        // Если время вышло, можем выполнить какие-то действия или остановить таймер
        if (remainTime <= 0)
        {
            // Действия при окончании времени
            timerOClock.text = "Time's Up";
            timerStarted = false; // Останавливаем таймер
            return;
        }

        // Делаем что-то с оставшимся временем, например, обновляем UI таймера
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        // Здесь обновляем UI таймера, включая отображение дней
        TimeSpan time = TimeSpan.FromSeconds(remainTime);
        string timerText = string.Format("{0}d. {1:D2}h. {2:D2}m. {3:D2}s.", (int)time.TotalDays, time.Hours, time.Minutes, time.Seconds);
        timerOClock.text = timerText;
    }
}
