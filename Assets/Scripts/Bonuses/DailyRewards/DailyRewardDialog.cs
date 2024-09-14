using System;
using CodeBase;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Bonuses.DailyRewards
{
    public class DailyRewardDialog : ClosableDialog
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private Button _acceptRewardButton;

        public event Action OnRewardAccepted;

        public void SetDayNumber(int day)
        {
            _text.SetText("Your day number: " + day);
        }

        protected override void OnEnable()
        {
            _acceptRewardButton.onClick.AddListener(AcceptReward);
        }

        private void AcceptReward()
        {
            OnRewardAccepted?.Invoke();
        }

        protected override void OnDisable()
        {
            _acceptRewardButton.onClick.RemoveListener(AcceptReward);
        }
    }
}