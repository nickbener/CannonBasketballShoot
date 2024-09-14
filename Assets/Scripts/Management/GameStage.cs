using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Input;
using DG.Tweening;
using Factories;
using Gameplay;
using Gameplay.Environment;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Management
{
    public class GameStage
    {
        private readonly LinkedList<Basket> _baskets = new LinkedList<Basket>();
        private readonly LinkedList<Star> _stars = new LinkedList<Star>();
        private readonly Stack<Action> _onActivateActions = new Stack<Action>(); 
        
        private readonly BasketFactory _basketFactory;
        private readonly StarFactory _starFactory;
        
        public int ActivationGoalsCount { get; private set; }
        
        public GameStage(int activationGoalsCount, BasketFactory basketFactory, StarFactory starFactory)
        {
            ActivationGoalsCount = activationGoalsCount;
            _basketFactory = basketFactory;
            _starFactory = starFactory;
        }

        public GameStage AddStar(Vector2 position)
        {
            _onActivateActions.Push(() =>
            {
                _stars.AddLast(_starFactory.CreateStar(position));
            });
            
            return this;
        }

        public GameStage AddStaticBasket(BasketSide side, float y)
        {
            _onActivateActions.Push(() =>
            {
                _baskets.AddLast(_basketFactory.CreateBasket(side, y));
            });
            
            return this;
        }
        
        public GameStage AddDynamicBasket(BasketSide side, float startY, float endY, float motionDuration, Ease ease)
        {
            _onActivateActions.Push(() =>
            {
                _baskets.AddLast(_basketFactory.CreateBasket(side, startY, 
                    basket => basket.transform.DOMoveY(endY, motionDuration)
                        .SetEase(ease)
                        .SetLoops(-1, LoopType.Yoyo)
                        .SetLink(basket.gameObject)));
            });
            
            return this;
        }

        public void Activate()
        {
            while (_onActivateActions.Count > 0)
            {
                Action action = _onActivateActions.Pop();
                action?.Invoke();
            }
            
            foreach (Basket basket in _baskets)
            {
                basket.DoShow();
            }
        }
        
        public void Release()
        {
            foreach (Basket basket in _baskets)
            {
                basket.DoHide().OnComplete(() =>
                {
                    _baskets.Remove(basket);
                    Object.Destroy(basket.gameObject);
                });
            }

            foreach (Star star in _stars)
            {
                Star.RemoveFromScene(star);
            }
            
            _stars.Clear();
        }

        public void RegisterBall(Ball ball)
        {
            foreach (Basket basket in _baskets)
            {
                basket.RegisterBall(ball);
            }
        }
    }
}