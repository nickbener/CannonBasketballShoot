using System;
using System.Collections.Generic;
using Infrastructure.Providers.DefaultConfigProvider;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Configs
{
    /// <summary>
    ///     Remote Settings
    /// </summary>
    public static class Remote
    {
        public static event Action OnInitializeAny;
        public static event Action OnInitializeDefault;
        public static event Action OnInitializeRemote;

        public static LevelConfig LevelConfig { get; private set; }
        public static CannonConfig CannonConfig { get; private set; }
        public static LeaderBoardConfig LeaderBoardConfig { get; private set; }
        public static ShopConfig ShopConfig { get; private set; }

        private static IDictionary<string, JToken> _cachedDefaultConfig;
        private static IDictionary<string, JToken> _remoteConfig;

        private static bool _hasInitializedByRemote;

        public static void InitializeByDefault(IDefaultConfigProvider defaultConfigProvider)
        {
            defaultConfigProvider.ClearCache();
            _cachedDefaultConfig = defaultConfigProvider.CachedConfig;

            if (_hasInitializedByRemote) return;

            ParseConfigs();
            OnInitializeDefault?.Invoke();
            OnInitializeAny?.Invoke();
            if (!defaultConfigProvider.IsRemoteConfigEnabled) OnInitializeRemote?.Invoke();
        }

        public static void InitializeByRemote(IDictionary<string, JToken> remoteConfig)
        {
            _hasInitializedByRemote = true;
            _remoteConfig = remoteConfig;
            ParseConfigs();
            OnInitializeRemote?.Invoke();
            OnInitializeAny?.Invoke();
        }

        private static void ParseConfigs()
        {
            LevelConfig = Parse<LevelConfig>(ConfigType.LevelConfig);
            CannonConfig = Parse<CannonConfig>(ConfigType.CannonConfig);
            LeaderBoardConfig = Parse<LeaderBoardConfig>(ConfigType.LeaderBoardConfig);
            ShopConfig = Parse<ShopConfig>(ConfigType.ShopConfig);
        }

        private static T Parse<T>(string type) where T : IConfig
        {
            try
            {
                return InternalParse(_remoteConfig ?? _cachedDefaultConfig);
            }
            catch (Exception e)
            {
                return InternalParse(_cachedDefaultConfig);
            }

            T InternalParse(IDictionary<string, JToken> config)
            {
                var configString = config[type];

                return JsonConvert.DeserializeObject<T>(configString.ToString());
            }
        }
    }
}