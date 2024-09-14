using AYellowpaper.SerializedCollections;
using RuntimeDebug.ConsoleMethods;
using UnityEngine;
using UnityEngine.Purchasing;

public class IAPManager : MonoBehaviour, IStoreListener
{
    [SerializeField] private PurchaseListener _purchaseListener;
    [SerializeField] private SerializedDictionary<string, ProductType> _productsId;

    private IStoreController StoreController { get; set; }
    private IExtensionProvider _storeExtensionProvider;

    private static bool _isInitialized;

    private void Start()
    {
        if (_isInitialized)
            return;
        _isInitialized = true;
        IAPInitialization();
    }

    private void IAPInitialization()
    {
        Debug.Log("Begin init IAP");

        ConfigurationBuilder builder = ConfigurationBuilder.Instance(StandardPurchasingModule.Instance());
        foreach (var item in _productsId)
        {
            builder.AddProduct(item.Key, item.Value);
        }

        Debug.Log("Builder added products");
        UnityPurchasing.Initialize(this, builder);
        Debug.Log("UnityPurchasing successful Initialize");
    }

    public bool IsIAPInitialized()
    {
        return StoreController != null && _storeExtensionProvider != null;
    }

    public PurchaseProcessingResult ProcessPurchase(PurchaseEventArgs purchaseEvent)
    {
        _purchaseListener.SuccessPurchased(purchaseEvent.purchasedProduct.definition.id);
        return PurchaseProcessingResult.Complete;
    }

    public void OnPurchaseFailed(Product product, PurchaseFailureReason failureReason)
    {

    }
    public void OnInitializeFailed(InitializationFailureReason error)
    {

    }

    public void OnInitializeFailed(InitializationFailureReason error, string message)
    {

    }

    public void OnInitialized(IStoreController controller, IExtensionProvider extensions)
    {
        StoreController = controller;
        _storeExtensionProvider = extensions;
    }

    //public static void BuyProductID(string productId, Action<string> action)
    public void BuyProductID(string productId)
    {
        Debug.Log("Try to buy: " + productId);
        if (Tools.IsDebug)
        {
            _purchaseListener.SuccessPurchased(productId);
            //action?.Invoke(productId);
            return;
        }

        if (!IsIAPInitialized()) return;
        Product product = StoreController.products.WithID(productId);
        if (product is { availableToPurchase: true })
        {
            StoreController.InitiatePurchase(product);
        }
    }


    public string GetPriceForId(string id)
    {
        return StoreController.products.WithStoreSpecificID(id).metadata.localizedPriceString;
    }

    public string GetDescriptionForId(string id)
    {
        return StoreController.products.WithStoreSpecificID(id).metadata.localizedDescription;
    }
}
