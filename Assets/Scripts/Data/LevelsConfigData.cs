using System.Collections.Generic;
using System;

namespace Editor.LevelEditor
{
    [Serializable]
    public class LevelsConfigData
    {
        public List<LevelData> Levels = new();
    }
}
