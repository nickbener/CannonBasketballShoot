using UnityEngine;

namespace CodeBase
{
    public class Dialog : MonoBehaviour
    {
        public void Hide()
        {
            Destroy(gameObject);
        }
    }
}