using CodeBase;
using ResourceSystem;
using System;
using UI.ResourceView;
using UnityEngine;
using Utils.Extensions;

public class BonusResourceView : ResourceView
{

    [SerializeField] private GameObject bonusBuyer;
    [SerializeField] private ChooseBoosterConfig _chooseBoosterConfig;

    public void Initialize(ResourceSystemService resourceSystemService, EventBusService eventBusService, ChooseBoosterConfig chooseBoosterConfig)
    {
        base.Initialize(resourceSystemService, eventBusService);
        _chooseBoosterConfig = chooseBoosterConfig;
        _text.text = StringExtensions.GetAdaptedInt((uint)resourceSystemService.GetResourceAmount(_type));
        if ((uint)resourceSystemService.GetResourceAmount(_type) <= 0 && !_chooseBoosterConfig.BustSelected[_type])
        {
            bonusBuyer.SetActive(true);
        }
        else
        {
            bonusBuyer.SetActive(false);
        }
    }

    protected override void OnResourceUpdated(object sender, EventArgs args)
    {
        if (args is ResourceChangedEventArgs)
        {
            if (_text == null)
                return;
            ResourceChangedEventArgs eventArgs = args as ResourceChangedEventArgs;
            if (eventArgs.ResourceType == _type)
            {
                _text.text = StringExtensions.GetAdaptedInt((uint)eventArgs.NewValue);
                if((uint)eventArgs.NewValue <= 0 && !_chooseBoosterConfig.BustSelected[_type])
                {
                    bonusBuyer.SetActive(true);
                }
                else
                {
                    bonusBuyer.SetActive(false);
                }
            }
        }
    }
}