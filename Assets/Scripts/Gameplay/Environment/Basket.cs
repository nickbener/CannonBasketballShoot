using System;
using DG.Tweening;
using UnityEngine;

namespace Gameplay.Environment
{
    public delegate Tween BasketBehaviour(Basket basket);
    
    public class Basket : MonoBehaviour
    {
        [SerializeField] private Cloth _net;
        [SerializeField] private BasketType _type;
        [SerializeField] private GameObject _trajectoryPointContainer;
        
        private BasketBehaviour _showBehaviour;
        private BasketBehaviour _hideBehaviour;
        private BasketBehaviour _customBehaviour;
        private BasketSide _side;
        private Tween _customBehaviourTween;

        public GameObject TrajectoryContainer => _trajectoryPointContainer;
        public BasketType Type => _type;
        public BasketSide Side
        {
            get { return _side; }
            set { _side = value; }
        }
        public void SetShowHideBehaviours(BasketBehaviour show, BasketBehaviour hide)
        {
            _showBehaviour = show;
            _hideBehaviour = hide;
        }

        public void SetCustomBehaviour(BasketBehaviour behaviour)
        {
            _customBehaviour = behaviour;
        }
        
        public Tween DoShow()
        {
            return _showBehaviour?.Invoke(this).OnComplete(() =>
            {
                _customBehaviourTween = _customBehaviour?.Invoke(this);
            });
        }
        
        public Tween DoHide()
        {
            _customBehaviourTween?.Kill();
            return _hideBehaviour?.Invoke(this);
        }
        
        public void RegisterBall(Ball ball)
        {
            ClothSphereColliderPair[] cachedSphereColliders = _net.sphereColliders;
            int oldLength = cachedSphereColliders.Length;
            
            Array.Resize(ref cachedSphereColliders, oldLength + 1);
            cachedSphereColliders[oldLength] = ball.SphereColliderPair;
            _net.sphereColliders = cachedSphereColliders;
        }

        public enum BasketType
        {
            side = 0,
            front = 1
        }
        
    }
}