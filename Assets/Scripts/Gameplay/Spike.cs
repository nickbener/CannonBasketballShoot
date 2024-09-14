using Gameplay.Environment;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] private GameObject _blockBox;
    [SerializeField] private BorderType _type;

    public GameObject BlockBox => _blockBox;


    public BorderType Type
    {
        get { return _type; }
        set { _type = value; }
    }
}
