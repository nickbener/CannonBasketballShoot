using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Configs
{
    [Serializable][JsonObject]
    public class LeaderBoardConfig : IConfig
    {
        [JsonProperty] public int LeaderBoardMinChanges;
        [JsonProperty] public int LeaderBoardMaxChanges;
    }
}