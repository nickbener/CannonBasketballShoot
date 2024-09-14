using System.Collections.Generic;
using CodeBase;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "dialogs_container", menuName = "Containers/DialogsContainer", order = 0)]
    public class DialogsContainer : ScriptableObject
    {
        [SerializeField] private List<Dialog> _dialogPrefabs;

        public TDialog GetDialogPrefab<TDialog>() where TDialog : Dialog
        {
            foreach (Dialog dialogPrefab in _dialogPrefabs)
            {
                if (dialogPrefab is TDialog concreteDialogPrefab)
                {
                    return concreteDialogPrefab;
                }
            }

            return null;
        }



    }
}