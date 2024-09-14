using Bonuses.DailyRewards;
using CodeBase;
using Configs;
using Cysharp.Threading.Tasks;
using DG.Tweening;
using Editor.LevelEditor;
using Factories;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using Infrastructure.Providers;
using Infrastructure.Providers.DefaultConfigProvider;
using Infrastructure.Services.Analytics;
using Management.Roots;
using Newtonsoft.Json.Linq;
using ResourceSystem;
using Services.Audio;
using Services.Data;
using Services.Data.Crypto.ROS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TriInspector;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using Utilities;
using Zenject;

namespace Management
{
    public class CompositionRoot : MonoBehaviour
    {
        [SerializeField] private Canvas _dialogsCanvas;
        [SerializeField] private RewardedAdManager _rewardedAdManager;
        [SerializeField] private InterstitialAdvertisement _interstitialAdvertisement;
        [SerializeField] private PurchaseListener _purchaseListener;
        [field: SerializeField] public DialogFactory DialogFactory { get; private set; }

        [Space]
        [SerializeField] private DefaultConfigProvider _defaultConfigProvider;

        public static CompositionRoot Instance { get; private set; }

        private EventBusService _eventBusService;
        private ResourceSystemService _resourceService;
        private GameplaySceneRoot _gameplaySceneRoot;
        private LevelsSceneRoot _levelsSceneRoot;
        private RatingSceneRoot _ratingSceneRoot;
        private DataService _dataService;
        private AudioService _audioService;
        private LevelDataProvider _levelDataProvider;
        private PlayerDataProvider _playerDataProvider;
        private IAnalyticsLogService _analyticsLogService;

        private DailyRewardService _dailyRewardService;

        public ResourceSystemService ResourceService => _resourceService;

        [Inject]
        public void Inject(DataService dataService, LevelDataProvider levelDataProvider, PlayerDataProvider playerDataProvider, IAnalyticsLogService analyticsLogService)
        {
            Debug.Log("Inject CompositionRoot");
            _dataService = dataService;
            _levelDataProvider = levelDataProvider;
            _playerDataProvider = playerDataProvider;
            _analyticsLogService = analyticsLogService;
        }

        private async void Awake()
        {
            if (Instance != null)
                return;
            Instance = this;

            DOTween.SetTweensCapacity(100, 25);

            _eventBusService = new EventBusService();
            _resourceService = new ResourceSystemService(_eventBusService, _dataService);
            _audioService = new AudioService();

            _dailyRewardService = new DailyRewardService(_dataService.GetModel<DailyRewardsModel>(), DialogFactory);
            InitializeDefaulConfig();
            await InitializeRemoteConfig();
            _rewardedAdManager.Initialize(_resourceService);
            _interstitialAdvertisement.ShowInterstitialAd();
            DontDestroyOnLoad(gameObject);
        }

        private void OnApplicationFocus(bool focus)
        {
            if(!focus)
            {
                _playerDataProvider.SaveData.DatetimeLeaveGame = DateTime.Now;
                _playerDataProvider.SaveData.DemandSave();
            }
        }

        private void OnEnable()
        {
            SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        private async UniTask InitializeRemoteConfig()
        {
            //var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            //if (dependencyStatus != DependencyStatus.Available)
            //    return;
            await UniTask.WaitUntil(() => _analyticsLogService.IsInitialized);
            var fetchTask = FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero);
            fetchTask.ContinueWithOnMainThread(FetchComplete);
            //var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            //var info = remoteConfig.Info;
            //if (info.LastFetchStatus != LastFetchStatus.Success)
            //{
            //    return;
            //}

            //remoteConfig.ActivateAsync()
            //    .ContinueWithOnMainThread(
            //        _ =>
            //        {

            //            var processedDictionary = FirebaseRemoteConfig.DefaultInstance.AllValues.ToDictionary(
            //                keyValuePair => keyValuePair.Key,
            //                keyValuePair => JToken.Parse(keyValuePair.Value.StringValue)
            //            );

            //            Remote.InitializeByRemote(processedDictionary);
            //        });
        }
        private void FetchComplete(Task fetchTask)
        {
            if (!fetchTask.IsCompleted)
            {
                return;
            }

            var remoteConfig = FirebaseRemoteConfig.DefaultInstance;
            var info = remoteConfig.Info;
            if (info.LastFetchStatus != LastFetchStatus.Success)
            {
                Debug.Log(
                    $"{nameof(FetchComplete)} was unsuccessful\n{nameof(info.LastFetchStatus)}: {info.LastFetchStatus}");
                return;
            }

            remoteConfig.ActivateAsync()
                .ContinueWithOnMainThread(
                    _ =>
                    {
                        Debug.Log(
                            "Remote data loaded.");

                        //var processedDictionary = FirebaseRemoteConfig.DefaultInstance.AllValues.ToDictionary(
                        //    keyValuePair => keyValuePair.Key,
                        //    keyValuePair => JToken.Parse(keyValuePair.Value.StringValue)
                        //);
                        Dictionary<string, JToken> processedDictionary = new Dictionary<string, JToken>();
                        foreach (var item in FirebaseRemoteConfig.DefaultInstance.AllValues)
                        {
                            if (item.Key.Contains("config"))
                                processedDictionary[item.Key] = JToken.Parse(item.Value.StringValue);
                            else
                                processedDictionary[item.Key] = item.Value.StringValue;
                        }

                        Remote.InitializeByRemote(processedDictionary);
                    });
        }

        private void InitializeDefaulConfig()
        {
            Remote.InitializeByDefault(_defaultConfigProvider);
        }

        private void OnActiveSceneChanged(Scene previous, Scene next)
        {
            switch (next.name)
            {
                case ScenesMetadata.StartSceneName:

                    if (_gameplaySceneRoot != null)
                    {
                        _gameplaySceneRoot.Dispose();
                    }
                    break;

                case ScenesMetadata.GameplaySceneName:

                    _analyticsLogService.LogEvent($"Level_Start_{string.Format("{0:D4}", LevelSettings.SelectedLevel)}");
                    _gameplaySceneRoot = GameplaySceneRoot.FromCurrentScene();
                    _dialogsCanvas.worldCamera = _gameplaySceneRoot.MainCamera;
                    LevelData selectedLevel = _levelDataProvider.LevelData.Levels.FirstOrDefault(x => x.LevelNumber == LevelSettings.SelectedLevel);
                    if (selectedLevel == null)
                        selectedLevel = _levelDataProvider.LevelData.Levels[_levelDataProvider.LevelData.Levels.Count - 1];
                    _gameplaySceneRoot.Initialize(_eventBusService, _resourceService, _audioService, _dailyRewardService, selectedLevel, _playerDataProvider, _analyticsLogService, _rewardedAdManager, _interstitialAdvertisement, _purchaseListener);

                    break;

                case ScenesMetadata.LevelsSceneName:

                    _levelsSceneRoot = LevelsSceneRoot.FromCurrentScene();
                    _levelsSceneRoot.Initialize(_eventBusService, _resourceService, _audioService);

                    break;
                case ScenesMetadata.RatingSceneName:

                    _ratingSceneRoot = RatingSceneRoot.FromCurrentScene();
                    _ratingSceneRoot.Initialize(_resourceService);

                    break;
            }
        }

        [Button]
        private void ShowRewardDialog()
        {
            _dailyRewardService.ShowRewardIfNeed();
        }

        private void OnDisable()
        {
            SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }
    }
}