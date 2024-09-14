using System;
using System.Collections.Generic;
using UnityEngine;

namespace Editor.LevelEditor
{
    [Serializable]
    public class ThrowConfigData
    {
        public string throwNumber;
        [HideInInspector]
        public CannonConfigData Cannon = new();
        [HideInInspector]
        public List<BasketConfigData> Baskets = new();
        [HideInInspector]
        public List<SpikeConfigData> Spikes = new();
        [HideInInspector]
        public List<StarConfigData> Stars = new();
    }
}
