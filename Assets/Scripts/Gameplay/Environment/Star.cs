using System;
using DG.Tweening;
using ResourceSystem;
using UnityEditor;
using UnityEngine;

namespace Gameplay.Environment
{
    public class Star : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _renderer;

        [Header("Animation")]
        [SerializeField] private Color _endColor;

        private ResourceSystemService _resourceService;
        
        public Star Initialize(ResourceSystemService resourceService)
        {
            _resourceService = resourceService;
            return this;
        }

        private void Start()
        {
            //_renderer.DOColor(_endColor, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);

            transform.DOScale(1.15f, 1.0f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo).SetLink(gameObject);

        }

        public void Collect()
        {
            //_resourceService.AppendResourceAmount(ResourceType.Star, 1);
            Destroy(gameObject);
        }

        public static void RemoveFromScene(Star star)
        {
            if (star != null && star.gameObject != null)
            {
                Destroy(star.gameObject);
            }
        }
    }
}