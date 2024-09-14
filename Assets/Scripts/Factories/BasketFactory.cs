using DG.Tweening;
using Extensions;
using Gameplay.Environment;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "basket_factory", menuName = "Factories/BasketFactory", order = 0)]
    public class BasketFactory : ScriptableObject
    {
        [SerializeField] private Basket _defaultBasketPrefab;
        [SerializeField] private Basket _basketFrontPrefab;

        private const float TargetHiddenXLeft = -5.0f;
        private const float TargetVisibleXLeft = -1.55f;
        private const float TargetVisibleXRight = 1.55f;
        private const float TargetHiddenXRight = 5.0f;
        
        private const float TargetDepartureDuration = 0.5f;
        
        private static Vector2 GetHiddenBasketPosition(BasketSide side, float y)
        {
            return side == BasketSide.Left ? new Vector2(TargetHiddenXLeft, y) : new Vector2(TargetHiddenXRight, y);
        }

        private static Tween ShowLeftBasket(Basket basket)
        {
            return basket.transform.DOMoveX(TargetVisibleXLeft, TargetDepartureDuration).SetLink(basket.gameObject);
        }
        
        private static Tween HideLeftBasket(Basket basket)
        {
            return basket.transform.DOMoveX(TargetHiddenXLeft, TargetDepartureDuration).SetLink(basket.gameObject);
        }
        
        private static Tween ShowRightBasket(Basket basket)
        {
            return basket.transform.DOMoveX(TargetVisibleXRight, TargetDepartureDuration).SetLink(basket.gameObject);
        }
        
        private static Tween HideRightBasket(Basket basket)
        {
            return basket.transform.DOMoveX(TargetHiddenXRight, TargetDepartureDuration).SetLink(basket.gameObject);
        }

        public Basket CreateBasket(BasketSide side, float y, BasketBehaviour behaviour = null)
        {
            Basket basket = Instantiate<Basket>(_defaultBasketPrefab, GetHiddenBasketPosition(side, y), Quaternion.identity);
            
            if (side == BasketSide.Left)
            {
                basket.SetShowHideBehaviours(ShowLeftBasket, HideLeftBasket);
            }
            else if (side == BasketSide.Right)
            {
                basket.SetShowHideBehaviours(ShowRightBasket, HideRightBasket);
                
                Vector3 localScale = basket.transform.localScale;
                basket.transform.localScale = localScale.WithX(localScale.x * -1.0f);
            }
            
            basket.SetCustomBehaviour(behaviour);
            return basket;
        }

        public Basket CreateFrontBasket(Vector3 position, BasketBehaviour behaviour = null)
        {
            Basket basket = Instantiate<Basket>(_basketFrontPrefab, position, Quaternion.identity);

            basket.SetCustomBehaviour(behaviour);
            return basket;
        }
    }
}