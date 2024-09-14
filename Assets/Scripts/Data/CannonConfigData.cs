using UnityEngine;
using System;
using Gameplay.Environment;

namespace Editor.LevelEditor
{
    [Serializable]
    public class CannonConfigData
    {
        public BasketSide Type;
        public int IdCannonPivot;
        public int IdCannonMain;
    }
}
