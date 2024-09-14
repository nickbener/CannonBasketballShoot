using AYellowpaper.SerializedCollections;
using Newtonsoft.Json;
using Services.Data;
using System;
using System.Collections.Generic;

namespace Infrastructure.Providers
{
    public class PlayerDataProvider : IPlayerDataProvider
    {
        [MatchModelToFile("player_data.edm")]
        public class PlayerDataModel : BaseDataModel
        {
            [JsonProperty] public bool IsMusicOff;
            [JsonProperty] public bool IsRated;
            [JsonProperty] public bool IsTutorialCompleted;
            [JsonProperty] public bool IsTutorialBusterCompleted;
            [JsonProperty] public bool IsTutorialForceCompleted;
            [JsonProperty] public int IdAvatar = -1;
            [JsonProperty] public string Nickname;
            [JsonProperty] public SerializedDictionary<int, int> LevelsRecord = new();
            [JsonProperty] public List<string> UnconsumablePurchases  = new();
            [JsonProperty] public DateTime DatetimeLeaveGame;
            [JsonProperty] public bool IsStarterOfferTimerStarted;
            [JsonProperty] public DateTime StarterOfferStartTime;

        }
        private PlayerDataModel _saveData;
        public PlayerDataModel SaveData
        {
            get { return _saveData; }
            set
            {
                _saveData = value;
                _saveData.DemandSave();
            }
        }

        public PlayerDataProvider(DataService dataService)
        {
            _saveData = dataService.GetModel<PlayerDataModel>();
        }

        public void SaveDataToFile() => _saveData.DemandSave();
    }
}