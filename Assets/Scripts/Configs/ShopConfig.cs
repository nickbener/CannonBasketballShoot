using System;
using Newtonsoft.Json;

namespace Configs
{
    [Serializable][JsonObject]
    public class ShopConfig : IConfig
    {
        [JsonProperty] public float Shop_No_Ads_Cost;
        [JsonProperty] public float Shop_Coins_XS_Cost;
        [JsonProperty] public int Shop_Coins_XS_Value;
        [JsonProperty] public float Shop_Coins_M_Cost;
        [JsonProperty] public int Shop_Coins_M_Value;
        [JsonProperty] public float Shop_Coins_XL_Cost;
        [JsonProperty] public int Shop_Coins_XL_Value;
        [JsonProperty] public float Shop_Bundle_0001_Cost;
        [JsonProperty] public int Shop_Bundle_0001_Coins;
        [JsonProperty] public float Shop_Bundle_0002_Cost;
        [JsonProperty] public int Shop_Bundle_0002_Coins;
        [JsonProperty] public float Shop_Bundle_0003_Cost;
        [JsonProperty] public int Shop_Bundle_0003_Coins;
                       
        [JsonProperty] public float Shop_Starter_Pack_Cost;
        [JsonProperty] public int Shop_Starter_Pack_Coins;
        [JsonProperty] public int Shop_Starter_Pack_Shields;
        [JsonProperty] public int Shop_Starter_Pack_Control;
        [JsonProperty] public int Shop_Starter_Pack_Bullet_Time;
        [JsonProperty] public int Shop_Stater_Pack_Time;
                       
        [JsonProperty] public float Shop_If_Lose_Offer_0001_Cost;
        [JsonProperty] public int Shop_If_Lose_Offer_0001_Coins;
        [JsonProperty] public int Shop_If_Lose_Offer_0001_Shields;
        [JsonProperty] public int Shop_If_Lose_Offer_0001_Control;
        [JsonProperty] public int Shop_If_Lose_Offer_0001_Bullet_Time;
        [JsonProperty] public int Shop_If_Lose_Offer_0001_Time;
    }
}
