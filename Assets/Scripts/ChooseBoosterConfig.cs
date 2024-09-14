using ResourceSystem;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChooseBoosterConfig : MonoBehaviour
{
    private Dictionary<ResourceType, bool> _bustSelected = new()
    {
        [ResourceType.BustCannon] = false,
        [ResourceType.BustTime] = false,
        [ResourceType.BustDef] = false,
    };

    public Dictionary<ResourceType, bool> BustSelected
    {
        get { return _bustSelected; }
        set { _bustSelected = value; }
    }
    public GameObject cannonSprite;
    public GameObject timeSprite;
    public GameObject defSprite;
    public TextMeshProUGUI levelText;
    private ResourceSystemService _resourceSystemService;


    public void Initialize(ResourceSystemService resourceService)
    {
        _resourceSystemService = resourceService;
    }

    public void SetLevelText(int level)
    {
        levelText.text = $"level {level}";
    }
    public void CannonBoolChange()
    {
        if(cannonSprite.activeSelf)
        {
            _bustSelected[ResourceType.BustCannon] = false;
            _resourceSystemService.AppendResourceAmount(ResourceType.BustCannon, 1);
            cannonSprite.SetActive(false);
        }
        else
        {
            _bustSelected[ResourceType.BustCannon] = true;
            _resourceSystemService.SubtractResourceAmount(ResourceType.BustCannon, 1);
            cannonSprite.SetActive(true);
        }
    }

    public void TimeBoolChange()
    {
        if (timeSprite.activeSelf)
        {
            _bustSelected[ResourceType.BustTime] = false;
            _resourceSystemService.AppendResourceAmount(ResourceType.BustTime, 1);
            timeSprite.SetActive(false);
        }
        else
        {
            _bustSelected[ResourceType.BustTime] = true;
            _resourceSystemService.SubtractResourceAmount(ResourceType.BustTime, 1);
            timeSprite.SetActive(true);
        }
    }

    public void DefBoolChange()
    {
        if (defSprite.activeSelf)
        {
            _bustSelected[ResourceType.BustDef] = false;
            _resourceSystemService.AppendResourceAmount(ResourceType.BustDef, 1);
            defSprite.SetActive(false);
        }
        else
        {
            _bustSelected[ResourceType.BustDef] = true;
            _resourceSystemService.SubtractResourceAmount(ResourceType.BustDef, 1);
            defSprite.SetActive(true);
        }
    }
}