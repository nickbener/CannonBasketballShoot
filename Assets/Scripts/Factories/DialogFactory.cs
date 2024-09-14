using CodeBase;
using UI.Popups;
using UnityEngine;

namespace Factories
{
    public class DialogFactory : MonoBehaviour
    {
        [SerializeField] private RectTransform _dialogsParent;
        [SerializeField] private DialogsContainer _dialogsContainer;

        public TDialog ShowDialog<TDialog>() where TDialog : Dialog
        {
            TDialog dialogPrefab = _dialogsContainer.GetDialogPrefab<TDialog>();

            if (dialogPrefab != null)
            {
                return Instantiate<TDialog>(dialogPrefab, _dialogsParent, false);
            }
            else
            {
                return null;
            }
        }
    }
}