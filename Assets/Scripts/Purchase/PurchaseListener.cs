using Configs;
using Cysharp.Threading.Tasks;
using Infrastructure.Providers;
using Management;
using Newtonsoft.Json.Linq;
using ResourceSystem;
using System;
using System.Net.Http;
using UnityEngine;
using UnityEngine.Purchasing;
using Zenject;

public class PurchaseListener : MonoBehaviour
{
    public event Action<string> PurchaseCompleted;
    [SerializeField] private CompositionRoot _compositionRoot;

    private PlayerDataProvider _playerDataProvider;
    private int _gold;
    private int _bustCannon;
    private int _bustTime;
    private int _bustDef;

    [Inject]
    private void Inject(PlayerDataProvider playerDataProvider)
    {
        _playerDataProvider = playerDataProvider;
    }

    public void SuccessPurchased(string id)
    {
        switch (id)
        {
            case "com.gamezmonster.cannonbasketball.starteroffer":
                _gold = Remote.ShopConfig.Shop_Starter_Pack_Coins;
                _bustCannon = Remote.ShopConfig.Shop_Starter_Pack_Control;
                _bustTime = Remote.ShopConfig.Shop_Starter_Pack_Bullet_Time;
                _bustDef = Remote.ShopConfig.Shop_Starter_Pack_Shields;
                AddResouces();
                UnConsumablePurchased(id);
                break;
            case "com.gamezmonster.cannonbasketball.loseoffer0001":
                _gold = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Coins;
                _bustCannon = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Control;
                _bustTime = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Bullet_Time;
                _bustDef = Remote.ShopConfig.Shop_If_Lose_Offer_0001_Shields;
                AddResouces();
                break;
            default:
                break;
        }
        PurchaseCompleted?.Invoke(id);
        Debug.Log("SuccessPurchased");
    }

    private void AddResouces()
    {
        _compositionRoot.ResourceService.AppendResourceAmount(ResourceType.Gold, _gold);
        _compositionRoot.ResourceService.AppendResourceAmount(ResourceType.BustCannon, _bustCannon);
        _compositionRoot.ResourceService.AppendResourceAmount(ResourceType.BustTime, _bustTime);
        _compositionRoot.ResourceService.AppendResourceAmount(ResourceType.BustDef, _bustDef);
    }

    private string GetLastPartId(string id)
    {
        string[] partsId = id.Split('.');
        string idEnd = partsId[partsId.Length - 1];
        return idEnd;
    }

    public void OnPurchaseFetched(ProductCollection product)
    {
        Debug.Log(product);
    }

    private void UnConsumablePurchased(string id)
    {
        _playerDataProvider.SaveData.UnconsumablePurchases.Add(id);
    }

    private async UniTask<DateTime> GetServerTime()
    {
        var client = new HttpClient();
        var responseBody = await client.GetStringAsync("http://worldtimeapi.org/api/timezone/Europe/Moscow");
        var response = JObject.Parse(responseBody);
        return DateTime.Parse(response["datetime"].ToString());
    }
}
