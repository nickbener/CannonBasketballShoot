using System;
using Newtonsoft.Json;

namespace Configs
{
    [Serializable]
    public class CannonConfig : IConfig
    {
        [JsonProperty] public long CannonForceShoot;
        [JsonProperty] public long CannonWaySpeed;
        [JsonProperty] public long ShieldWaySpeed;
        [JsonProperty] public long BallFallSpeed;
    }
}