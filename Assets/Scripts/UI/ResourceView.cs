using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using ResourceSystem;
using Utils.Extensions;
using CodeBase;
using System.Diagnostics.Tracing;

namespace UI.ResourceView
{
    public class ResourceView : MonoBehaviour
    {
        [SerializeField] protected ResourceType _type;
        [SerializeField] protected TextMeshProUGUI _text;

        private ResourceSystemService _resourceSystemService;
        private EventBusService _eventBusService;

        public virtual void Initialize(ResourceSystemService resourceSystemService, EventBusService eventBusService)
        {
            _resourceSystemService = resourceSystemService;
            _eventBusService = eventBusService;
            _text.text = StringExtensions.GetAdaptedInt((uint)_resourceSystemService.GetResourceAmount(_type));
            

            SubscribeEvents();
        }

        private void OnDestroy()
        {
            UnsubscriveEvents();
        }

        public void AddGold()
        {
            _resourceSystemService.AppendResourceAmount(_type, 1000);
        }

        private void SubscribeEvents()
        {
            switch (_type)
            {
                case ResourceType.Gold:
                    _eventBusService.AddListener(GameEventKey.ResourceCountChanged, OnResourceUpdated);
                    break;
                case ResourceType.BustCannon:
                    _eventBusService.AddListener(GameEventKey.ResourceCountChanged, OnResourceUpdated);
                    break;
                case ResourceType.BustTime:
                    _eventBusService.AddListener(GameEventKey.ResourceCountChanged, OnResourceUpdated);
                    break;
                case ResourceType.BustDef:
                    _eventBusService.AddListener(GameEventKey.ResourceCountChanged, OnResourceUpdated);
                    break;
                case ResourceType.Star:
                    _eventBusService.AddListener(GameEventKey.ResourceCountChanged, OnResourceUpdated);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void UnsubscriveEvents()
        {
            _eventBusService.RemoveListener(GameEventKey.ResourceCountChanged, OnResourceUpdated);
        }

        protected virtual void OnResourceUpdated(object sender, EventArgs args)
        {
            if(args is ResourceChangedEventArgs)
            {
                ResourceChangedEventArgs eventArgs = args as ResourceChangedEventArgs;
                if (eventArgs.ResourceType == _type)
                    _text.text = StringExtensions.GetAdaptedInt((uint)eventArgs.NewValue);
            }
        }
    }
}