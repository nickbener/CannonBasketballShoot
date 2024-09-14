using Configs;

public class LoseOfferPopup : OfferPopup
{
    new private void Awake()
    {
        base.Awake();
        coins_count.text = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Coins.ToString();
        cannonBonus_count.text = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Control.ToString();
        timerBonus_count.text = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Bullet_Time.ToString();
        shieldBonus_count.text = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Shields.ToString();
        cost.text = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Cost.ToString() + "$";

        startTimeHours = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Time;
    }
}
