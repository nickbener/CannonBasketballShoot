using System.Collections.Generic;
using UnityEngine;

public class BonusManager : MonoBehaviour
{
    [SerializeField] private GameObject _cannonBoostBorder;
    [SerializeField] private GameObject _timeBoostBorder;
    [SerializeField] private GameObject _defBoostBorder;
    private List<GameObject> _boxInSpike = new();
    private bool _cannonBoostIsActive;

    public List<GameObject> BoxInSpike
    {
        get { return _boxInSpike; }
        set { _boxInSpike = value; }
    }

    public bool CannonBoostIsActive => _cannonBoostIsActive;

    private void Start()
    {
    }

    internal void Initialize()
    {
        if (!LevelSettings.BustSelected[ResourceSystem.ResourceType.BustCannon]
            && !LevelSettings.BustSelected[ResourceSystem.ResourceType.BustTime]
            && !LevelSettings.BustSelected[ResourceSystem.ResourceType.BustDef])
            gameObject.SetActive(false);
        CannonButton(LevelSettings.BustSelected[ResourceSystem.ResourceType.BustCannon]);
        TimeButton(LevelSettings.BustSelected[ResourceSystem.ResourceType.BustTime]);
        ShieldButton(LevelSettings.BustSelected[ResourceSystem.ResourceType.BustDef]);
    }

    public void CannonButton(bool isActive)
    {
        if (!isActive)
            return;
        _cannonBoostBorder.SetActive(true);
        _cannonBoostIsActive = true;
    }

    public void TimeButton(bool isActive)
    {
        Time.timeScale = 1f;
        if (!isActive)
            return;
        _timeBoostBorder.SetActive(true);
        Time.timeScale = 0.5f;
    }

    public void ShieldButton(bool isActive)
    {
        if (!isActive)
            return;
        _defBoostBorder.SetActive(true);
        for (int i = 0; i < _boxInSpike.Count; i++)
        {
            _boxInSpike[i].SetActive(true);
        }
    }
}
