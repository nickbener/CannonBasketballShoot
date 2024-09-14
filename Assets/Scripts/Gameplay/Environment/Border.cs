using UnityEngine;

namespace Gameplay.Environment
{
    public class Border : MonoBehaviour
    {
        [field: SerializeField] public BorderType Type { get; private set; }
    }
    public enum BorderType
    {
        None = 0,
        Top = 1,
        Bottom = 2,
        Left = 3,
        Right = 4,
    }
}