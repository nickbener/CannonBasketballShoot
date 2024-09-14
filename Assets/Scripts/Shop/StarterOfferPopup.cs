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

        int baseTime = Remote.ShopConfig.Shop_Stater_Pack_Time * 3600; // ������������ ���� � �������
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
        // ���������, ���� �� ��� ��������� ����� ������ �������
        if (!timerStarted)
            return;

        remainTime -= Time.deltaTime;

        // ���� ����� �����, ����� ��������� �����-�� �������� ��� ���������� ������
        if (remainTime <= 0)
        {
            // �������� ��� ��������� �������
            timerOClock.text = "Time's Up";
            timerStarted = false; // ������������� ������
            return;
        }

        // ������ ���-�� � ���������� ��������, ��������, ��������� UI �������
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        // ����� ��������� UI �������, ������� ����������� ����
        TimeSpan time = TimeSpan.FromSeconds(remainTime);
        string timerText = string.Format("{0}d. {1:D2}h. {2:D2}m. {3:D2}s.", (int)time.TotalDays, time.Hours, time.Minutes, time.Seconds);
        timerOClock.text = timerText;
    }
}
