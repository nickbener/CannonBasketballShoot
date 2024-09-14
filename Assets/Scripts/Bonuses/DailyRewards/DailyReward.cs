using System;

namespace Bonuses.DailyRewards
{
    [Serializable]
    public struct DailyReward
    {
        public int Id { get; set; }
        public long ReceivingTimeTicks { get; set; }

        public bool IsReceived => ReceivingTimeTicks > 0L;
    }
}