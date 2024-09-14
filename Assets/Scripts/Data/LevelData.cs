using System;
using System.Collections.Generic;

namespace Editor.LevelEditor
{
    [Serializable]
    public class LevelData
    {
        public int LevelNumber;
        public int idBackgroundSprite;
        public List<ThrowConfigData> Throws = new();
    }
}
