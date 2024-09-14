using Configs;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StartOfferTextControl : MonoBehaviour
{
    public TextMeshProUGUI coins_count;
    public TextMeshProUGUI cannonBonus_count;
    public TextMeshProUGUI timerBonus_count;
    public TextMeshProUGUI shieldBonus_count;
    public TextMeshProUGUI cost;
    public TextMeshProUGUI timerOClock;

    void Update()
    {
        coins_count.text = Remote.ShopConfig.Shop_Starter_Pack_Coins.ToString();
        cannonBonus_count.text = Remote.ShopConfig.Shop_Starter_Pack_Control.ToString();
        timerBonus_count.text = Remote.ShopConfig.Shop_Stater_Pack_Time.ToString();
        shieldBonus_count.text = Remote.ShopConfig.Shop_Starter_Pack_Shields.ToString() + "$";
        cost.text = Remote.ShopConfig.Shop_Starter_Pack_Cost.ToString();
        timerOClock.text = Remote.ShopConfig.Shop_Starter_Pack_Bullet_Time.ToString();
    }
}
