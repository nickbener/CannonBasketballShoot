using UnityEngine;
using UnityEngine.UI;

namespace CodeBase
{
    public class ClosableDialog : Dialog
    {
        [SerializeField] private Button _hideButton;
        [SerializeField] private Button _hideArea;

        protected virtual void OnEnable()
        {
            _hideArea.onClick.AddListener(Hide);
            _hideButton.onClick.AddListener(Hide);
        }
        
        protected virtual void OnDisable()
        {
            _hideButton.onClick.RemoveListener(Hide);
            _hideArea.onClick.RemoveListener(Hide);
        }
    }
}