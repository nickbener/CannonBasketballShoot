using System;
using System.ComponentModel.Design;
using Bonuses.DailyRewards;
using CodeBase;
using Configs;
using Editor.LevelEditor;
using Factories;
using Gameplay;
using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using ResourceSystem;
using Services.Audio;
using UI;
using UI.ResourceView;
using UnityEngine;

namespace Management.Roots
{
    public class GameplaySceneRoot : MonoBehaviour, IDisposable
    {
        [SerializeField] private BonusManager _bonusManager;
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private BasketFactory _basketFactory;
        [SerializeField] private StarFactory _starFactory;
        [SerializeField] private VisualEffectFactory _vfxFactory;
        [SerializeField] private ScoreBoard _scoreBoard;
        [SerializeField] private Cannon _cannon;
        [SerializeField] private ResourceView _goldView;
        [SerializeField] private ResourceView _starView;
        [SerializeField] private PopupManager _popupManager;
        [SerializeField] private ShopView _shopView;
        [SerializeField] private LevelEditorSettings _levelEditorSettings;
        [SerializeField] private BasketContainer _basketContainer;
        [SerializeField] private SpikeContainer _spikeContainer;
        [SerializeField] private StarContainer _starContainer;
        [SerializeField] private SpriteRenderer _sceneBG;
        [SerializeField] private GameplayUI _gameplayUI;
        [SerializeField] private Settings _settings;

        private GameStateMachine _gameStateMachine;
        private ResourceSystemService _resourceService;
        private EventBusService _eventBusService;
        private int _amountStar = 0;

        public Camera MainCamera => _mainCamera;

        public ResourceSystemService ResourceService => _resourceService;
        public EventBusService EventBusService => _eventBusService;
        public GameStateMachine GameStateMachine  => _gameStateMachine;
        public LevelData LevelConfigData { get; set; }
        public int AmountStar
        {
            get { return _amountStar; }
            set {  _amountStar = value; }
        }

        public static GameplaySceneRoot FromCurrentScene()
        {
            return FindObjectOfType<GameplaySceneRoot>();
        }

        public GameplaySceneRoot Initialize(
            EventBusService eventBusService,
            ResourceSystemService resourceService,
            AudioService audioService,
            DailyRewardService dailyRewardService,
            LevelData levelConfigData,
            PlayerDataProvider playerDataProvider,
            IAnalyticsLogService analyticsLogService,
            RewardedAdManager rewardedAdManager,
            InterstitialAdvertisement interstitialAdvertisement,
            PurchaseListener purchaseListener)
        {
            LevelConfigData = levelConfigData;
            InitializeScene(levelConfigData, eventBusService, resourceService);
            _resourceService = resourceService;
            _eventBusService = eventBusService;
            _gameStateMachine = new GameStateMachine(eventBusService, resourceService, _basketFactory, _starFactory.Initialize(resourceService),
                _vfxFactory, _scoreBoard, _cannon, levelConfigData, playerDataProvider, this, analyticsLogService);
            _cannon.InjectDependencies(eventBusService, audioService);
            _goldView.Initialize(resourceService, eventBusService);
            _starView.Initialize(resourceService, eventBusService);
            _popupManager.Initialize(this, resourceService, rewardedAdManager, interstitialAdvertisement, purchaseListener);
            _gameplayUI.Initialize(this);
            dailyRewardService.ShowRewardIfNeed();
            _bonusManager.Initialize();
            _shopView.Initialize(resourceService);
            _settings.Initialize(resourceService);
            return this;
        }

        private void InitializeScene(LevelData levelConfigData, EventBusService eventBusService, ResourceSystemService resourceService)
        {
            _sceneBG.sprite = _levelEditorSettings.SpriteAsset.GetSprite("Background", $"{levelConfigData.idBackgroundSprite}");
            _cannon.Initialize(levelConfigData.Throws[0], _levelEditorSettings);
            _basketContainer.Initialize(levelConfigData, _levelEditorSettings, _basketFactory, eventBusService);
            _spikeContainer.Initialize(levelConfigData, _levelEditorSettings, eventBusService);
            _starContainer.Initialize(levelConfigData, _levelEditorSettings, resourceService, eventBusService);
        }

        public void Dispose()
        {
            _gameStateMachine?.Dispose();
        }
    }
}