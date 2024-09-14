using System.Collections.Generic;
using UnityEngine;
using System;
using Gameplay.Environment;

namespace Editor.LevelEditor
{
    [Serializable]
    public class BasketConfigData
    {
        [HideInInspector]
        public List<Vector3> Positions = new();
        [HideInInspector]
        public Basket.BasketType BasketType;
        [HideInInspector]
        public BasketSide BasketSide;
        public float cycleTime = 5;

        public BasketConfigData()
        {

        }

        public BasketConfigData(List<Vector3> positions)
        {
            Positions = positions;
        }
    }
}
