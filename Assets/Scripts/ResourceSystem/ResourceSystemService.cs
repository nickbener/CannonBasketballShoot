using System;
using CodeBase;
using Services.Data;
using UnityEngine;

namespace ResourceSystem
{
    public class ResourceSystemService
    {
        private EventBusService _eventBusService;
        private DataService _dataService;
        private ResourceSystemModel _model;
        
        public ResourceSystemService(EventBusService eventBusService, DataService dataService)
        {
            _eventBusService = eventBusService;
            _dataService = dataService;
            _model = _dataService.GetModel<ResourceSystemModel>();
            
            Debug.Log("Goals: " + _model.GetResourceAmount(ResourceType.Score));
        }

        public void SetResourceAmount(ResourceType resourceType, double amount)
        {
            double oldValue = _model.GetResourceAmount(resourceType);
            ChangeValueAndFire(resourceType, oldValue, amount);
        }

        public void AppendResourceAmount(ResourceType resourceType, double amount)
        {
            double oldValue = _model.GetResourceAmount(resourceType);
            double newValue = oldValue + amount;
            ChangeValueAndFire(resourceType, oldValue, newValue);
        }
        
        public void SubtractResourceAmount(ResourceType resourceType, double amount)
        {
            double oldValue = _model.GetResourceAmount(resourceType);
            double newValue = Math.Clamp(oldValue - amount, 0, double.MaxValue);
            
            ChangeValueAndFire(resourceType, oldValue, newValue);
        }

        public double GetResourceAmount(ResourceType resourceType)
        {
            return _model.GetResourceAmount(resourceType);
        }

        private void ChangeValueAndFire(ResourceType resourceType, double oldValue, double newValue)
        {
            _model.SetResourceAmount(resourceType, newValue);
            
            _eventBusService.Broadcast(GameEventKey.ResourceCountChanged, this, 
                new ResourceChangedEventArgs(resourceType, oldValue, newValue));
        }
        

        public void ClearAll()
        {
            
        }
    }
}