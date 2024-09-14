using UnityEngine;
using System;
using Gameplay.Environment;

namespace Editor.LevelEditor
{
    [Serializable]
    public class SpikeConfigData
    {
        public BorderType Side;
        public Vector3 Position;

        public SpikeConfigData()
        {

        }

        public SpikeConfigData(BorderType side, Vector3 position)
        {
            Side = side;
            Position = position;
        }
    }
}
