using CodeBase;
using ResourceSystem;
using Services.Audio;
using UI.ResourceView;
using UnityEngine;

public class LevelsSceneRoot : MonoBehaviour
{
    [SerializeField] private ResourceView _goldView;
    [SerializeField] private ShopView _shopView;
    [SerializeField] private BonusPopUpView _bonusPopUpView;
    [SerializeField] private Settings _settings;

    public static LevelsSceneRoot FromCurrentScene()
    {
        return FindObjectOfType<LevelsSceneRoot>();
    }

    public LevelsSceneRoot Initialize(
        EventBusService eventBusService,
        ResourceSystemService resourceService,
            AudioService audioService)
    {
        _goldView.Initialize(resourceService, eventBusService);
        _bonusPopUpView.Initialize(resourceService, eventBusService);
        _shopView.Initialize(resourceService);
        _settings.Initialize(resourceService);

        return this;
    }
}