using System;
using CodeBase;
using UnityEngine;
using UnityEngine.UI;

namespace Management
{
    public class LevelFailedDialog : Dialog
    {
        [SerializeField] private Button _retryButton;
        
        public event Action OnDemandRetry;
        
        private void OnEnable()
        {
            Time.timeScale = 0.0f;
            _retryButton.onClick.AddListener(Retry);
        }

        private void Retry()
        {
            OnDemandRetry?.Invoke();
        }

        private void OnDisable()
        {
            _retryButton.onClick.RemoveListener(Retry);
            Time.timeScale = 1.0f;
        }
    }
}