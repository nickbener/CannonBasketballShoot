using System;
using Factories;

namespace Bonuses.DailyRewards
{
    public class DailyRewardService
    {
        private readonly DailyRewardsModel _dataModel;
        private readonly DialogFactory _dialogsFactory;

        private DailyRewardDialog _cachedDialog;
        
        public DailyRewardService(DailyRewardsModel dataModel, DialogFactory dialogsFactory)
        {
            _dataModel = dataModel;
            _dialogsFactory = dialogsFactory;
        }

        public void ShowRewardIfNeed()
        {
            DailyReward? lastReceivedReward = _dataModel.GetLastReceivedReward();

            if (lastReceivedReward == null || IsTimeToShowNewReward(lastReceivedReward.Value))
            {
                _cachedDialog = _dialogsFactory.ShowDialog<DailyRewardDialog>();
                _cachedDialog.SetDayNumber(_dataModel.ReceivedRewardsCount + 1);
                _cachedDialog.OnRewardAccepted += AcceptReward;
            }
        }
        
        private static bool IsTimeToShowNewReward(in DailyReward lastReceivedReward)
        {
            DateTime receivingDateTime = new DateTime(lastReceivedReward.ReceivingTimeTicks);
            return DateTime.UtcNow.DayOfYear != receivingDateTime.DayOfYear;
        }

        private void AcceptReward()
        {
            DailyReward newDailyReward = new DailyReward()
            {
                Id = GetNewRewardId(), 
                ReceivingTimeTicks = DateTime.UtcNow.Ticks
            };
            
            _dataModel.AppendReceivedReward(newDailyReward);
            
            _cachedDialog.OnRewardAccepted -= AcceptReward;
            _cachedDialog.Hide();
        }

        private int GetNewRewardId()
        {
            DailyReward? lastReceivedReward = _dataModel.GetLastReceivedReward();

            if (lastReceivedReward != null)
            {
                return lastReceivedReward.Value.Id + 1;
            }
            else
            {
                return 0;
            }
        }


    }
}