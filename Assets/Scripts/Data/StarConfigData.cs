using System;
using UnityEngine;

namespace Editor.LevelEditor
{
    [Serializable]
    public class StarConfigData
    {
        public Vector3 Position;

        public StarConfigData()
        {

        }

        public StarConfigData(Vector3 position)
        {
            Position = position;
        }
    }
}
