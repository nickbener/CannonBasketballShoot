using System;
using System.Collections.Generic;
using Services.Data;
using Newtonsoft.Json;

namespace ResourceSystem
{
    [MatchModelToFile("resource_system.edm")]
    public class ResourceSystemModel : BaseDataModel
    {
        [JsonProperty] private Dictionary<ResourceType, double> _resources = new Dictionary<ResourceType, double>();
        
        public double GetResourceAmount(ResourceType resourceType)
        {
            if (_resources.TryGetValue(resourceType, out var amount))
            {
                return amount;
            }
            else
            {
                return 0.0;
            }
        }

        public void SetResourceAmount(ResourceType resourceType, double amount)
        {
            _resources[resourceType] = amount;
            
            DemandSave();
        }

        /*public override string ToString()
        {
            string s = "";
            
            foreach (var v in _resources)
            {
                s += v.Key + " " + v.Value;
            }

            return s;
        }*/
    }
}