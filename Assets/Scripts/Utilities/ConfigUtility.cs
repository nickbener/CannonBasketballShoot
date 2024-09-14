using System;
using Configs;
using JetBrains.Annotations;
using Newtonsoft.Json;
using TriInspector;
using UnityEngine;


namespace Editor.Utils
{
    public class ConfigUtility : ScriptableObject
    {
        public enum Type
        {
            LevelConfig,
            CannonConfig,
            LeaderBoardConfig,
            ShopConfig
        }

        [SerializeField] private Configs.LevelConfig _levelConfig;
        [SerializeField] private CannonConfig _cannonConfig;
        [SerializeField] private LeaderBoardConfig _leaderBoardConfig;
        [SerializeField] private ShopConfig _shopConfig;

        [UsedImplicitly][SerializeField] private string _configName;
        [SerializeField][TextArea(12, 9999)] private string _serialized;

        [Button]//[PropertyOrder(0)]
        public void ToJson(Type type)
        {
            switch (type)
            {
                case Type.LevelConfig:
                    _configName = ConfigType.LevelConfig;
                    _serialized = JsonConvert.SerializeObject(_levelConfig, Formatting.Indented);
                    break;
                case Type.CannonConfig:
                    _configName = ConfigType.CannonConfig;
                    _serialized = JsonConvert.SerializeObject(_cannonConfig, Formatting.Indented);
                    break;
                case Type.LeaderBoardConfig:
                    _configName = ConfigType.LeaderBoardConfig;
                    _serialized = JsonConvert.SerializeObject(_leaderBoardConfig, Formatting.Indented);
                    break;
                case Type.ShopConfig:
                    _configName = ConfigType.ShopConfig;
                    _serialized = JsonConvert.SerializeObject(_shopConfig, Formatting.Indented);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
        }
    }
}