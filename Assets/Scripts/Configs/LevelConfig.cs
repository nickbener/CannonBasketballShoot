using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Configs
{
    [Serializable][JsonObject]
    public class LevelConfigData
    {
        [JsonProperty] public int PlayerHPSum;
        [JsonProperty] public int LevelHPValue;
        [JsonProperty] public int LevelStarValue;
        [JsonProperty] public int LevelValue;
        [JsonProperty] public int HpCost;
    }

    [Serializable][JsonObject]
    public class LevelConfig : IConfig
    {
        [JsonProperty] public List<LevelConfigData> LevelConfigData;
    }
}
