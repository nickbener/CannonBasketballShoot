using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Services.Data;

namespace Bonuses.DailyRewards
{
    [MatchModelToFile("daily_rewards.edm")]
    public class DailyRewardsModel : BaseDataModel
    {
        [JsonProperty] private List<DailyReward?> _dailyRewards = new List<DailyReward?>();

        public int ReceivedRewardsCount => _dailyRewards.Count;
        
        public DailyReward? GetLastReceivedReward()
        {
            return _dailyRewards.Count > 0 ? _dailyRewards[_dailyRewards.Count - 1] : null;
        }
        
        public void AppendReceivedReward(DailyReward reward)
        {
            _dailyRewards.Add(reward);
            DemandSave();
        }

        // ---
        
        private DailyReward? SelectRewardById(int rewardId)
        {
            return _dailyRewards.Find(dailyReward => dailyReward != null && dailyReward.Value.Id == rewardId);
        }
        
        private void UpdateReward(DailyReward newRewardValue)
        {
            int foundIndex = _dailyRewards.FindIndex(dailyReward => dailyReward != null && dailyReward.Value.Id == newRewardValue.Id);

            if (foundIndex != -1)
            {
                _dailyRewards[foundIndex] = newRewardValue;
            }
        }
        
        public bool IsRewardReceived(int rewardId)
        {
            DailyReward? reward = SelectRewardById(rewardId);
            return reward?.IsReceived ?? false;
        }

        public void SetReceived(int rewardId, long receivingTimeTicks)
        {
            DailyReward? reward = SelectRewardById(rewardId);
            
            if (reward.HasValue)
            {
                DailyReward cachedValue = reward.Value;
                cachedValue.ReceivingTimeTicks = receivingTimeTicks;
                reward = cachedValue;
                UpdateReward(reward.Value);
                DemandSave();
            }
        }

    }
}