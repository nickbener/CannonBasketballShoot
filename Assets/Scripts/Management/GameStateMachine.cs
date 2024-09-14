using System;
using System.Collections.Generic;
using CodeBase;
using DG.Tweening;
using Editor.LevelEditor;
using Factories;
using Gameplay;
using Gameplay.Environment;
using Infrastructure.Providers;
using Infrastructure.Services.Analytics;
using Management.Roots;
using ResourceSystem;
using Services.Data;
using UI;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace Management
{
    public class GameStateMachine : IDisposable
    {
        public event Action LevelFailed;
        public event Action<int> LevelCompleted;
        public event Action<int> GoalScored;

        private EventBusService _eventBusService;
        private ResourceSystemService _resourceService;

        private BasketFactory _basketFactory;
        private StarFactory _starFactory;
        private VisualEffectFactory _vfxFactory;

        private List<GameStage> _stages;
        private GameStage _currentStage;

        private ScoreBoard _scoreBoard;
        private Cannon _cannon;

        private PlayerDataProvider _playerDataProvider;
        IAnalyticsLogService _analyticsLogService;
        private GameplaySceneRoot _gameplaySceneRoot;

        private int _scoreCounter;
        private bool _isGameOver;
        private int _throws;
        private int _currentLevel;
        private int _goalInRow;

        public Cannon Cannon => _cannon;

        public GameStateMachine(EventBusService eventBusService, ResourceSystemService resourceService,
            BasketFactory basketFactory, StarFactory starFactory, VisualEffectFactory vfxFactory, ScoreBoard scoreBoard, Cannon cannon, LevelData levelConfigData, PlayerDataProvider playerDataProvider, GameplaySceneRoot gameplaySceneRoot, IAnalyticsLogService analyticsLogService)
        {
            _analyticsLogService = analyticsLogService;
            _playerDataProvider = playerDataProvider;
            _eventBusService = eventBusService;
            _resourceService = resourceService;
            _gameplaySceneRoot = gameplaySceneRoot;

            _basketFactory = basketFactory;
            _starFactory = starFactory;
            _vfxFactory = vfxFactory;

            _scoreBoard = scoreBoard;
            _cannon = cannon;
            _throws = levelConfigData.Throws.Count;
            _currentLevel = levelConfigData.LevelNumber;

            //InitializeStages();
            //NextStage();

            _scoreBoard.UpdateView(_scoreCounter);
            //_scoreBoard.UpdateView((int)_resourceService.GetResourceAmount(ResourceType.Score));

            //_cannon.OnBallCreated.AddListener(OnBallCreated);
            _cannon.OnLastBallGrounded.AddListener(OnLevelFailed);
            _cannon.BallGrounded += OnBallGrounded;
            _eventBusService.AddListener(GameEventKey.GoalScored, OnGoalScored);
        }

        public void RestartLevel()
        {
            _isGameOver = false;
            _cannon.Recharge();
        }
        public void RechargeCannon()
        {
            _isGameOver = false;
            _cannon.Recharge();
        }
        public void RechargeCannonOnce()
        {
            _isGameOver = false;
            _cannon.RechargeOneBall();
        }

        private void OnBallGrounded()
        {
            _analyticsLogService.LogEvent($"Trow_Fail_{LevelSettings.SelectedLevel}_{_scoreCounter +1}");
            _goalInRow = 0;
        }

        private void OnBallCreated(Ball ball)
        {
            foreach (GameStage stage in _stages)
            {
                stage.RegisterBall(ball);
            }
        }

        public void OnLevelFailed()
        {
            if (!_isGameOver)
            {
                _isGameOver = true;
                LevelFailed?.Invoke();
            }
            //DOVirtual.DelayedCall(1.5f, () =>
            //{
            //    LevelFailedDialog failDialog = CompositionRoot.Instance.DialogFactory.ShowDialog<LevelFailedDialog>();
            //    failDialog.OnDemandRetry += () =>
            //    {
            //        RestartLevel();
            //        failDialog.Hide();
            //    };
            //});
        }

        private void OnGoalScored(object sender, EventArgs args)
        {
            _analyticsLogService.LogEvent($"Throw_Success_{LevelSettings.SelectedLevel}_{_scoreCounter+1}");
            _goalInRow += 1;
            GoalScored?.Invoke(_goalInRow);
            _cannon.Recharge();
            _scoreCounter++;
            _resourceService.AppendResourceAmount(ResourceType.Score, 1);
            _scoreBoard.UpdateView(_scoreCounter);
            //_scoreBoard.UpdateView((int)_resourceService.GetResourceAmount(ResourceType.Score));
            //_vfxFactory.CreateExclamation(new Vector2(0.0f, 1.0f), "Goal!");

            if (_scoreCounter >= _throws)
            {
                _isGameOver = true;
                if (_playerDataProvider.SaveData.LevelsRecord.ContainsKey(_currentLevel) && _gameplaySceneRoot.AmountStar > _playerDataProvider.SaveData.LevelsRecord[_currentLevel])
                    _playerDataProvider.SaveData.LevelsRecord[_currentLevel] = _gameplaySceneRoot.AmountStar;
                else
                    _playerDataProvider.SaveData.LevelsRecord[_currentLevel] = _gameplaySceneRoot.AmountStar;
                LevelCompleted?.Invoke(_gameplaySceneRoot.AmountStar);
                return;
            }

            //NextStage();
        }

        private GameStage CreateGameStage(int activationGoalsCount)
        {
            return new GameStage(activationGoalsCount, _basketFactory, _starFactory);
        }

        private void InitializeStages()
        {
            _stages = new List<GameStage>
            {
                //CreateGameStage(0).AddStaticBasket(BasketSide.Left, 0.915f),
                //CreateGameStage(1).AddStaticBasket(BasketSide.Left, -0.5f),
                //CreateGameStage(2).AddStaticBasket(BasketSide.Left, -1.0f).AddStar(Vector2.zero),
                //CreateGameStage(5).AddDynamicBasket(BasketSide.Left, -1.0f, 1.5f, 3.0f, Ease.Linear),
                //CreateGameStage(6).AddStaticBasket(BasketSide.Left,3.0f).AddStar(Vector2.one),
                //CreateGameStage(7)
                //    .AddDynamicBasket(BasketSide.Left,3.0f, -1.0f, 4.0f, Ease.Linear),

                //CreateGameStage(8)
                //    .AddStaticBasket(BasketSide.Left, 4.0f)
                //    .AddStaticBasket(BasketSide.Left, 1.0f)
                //    .AddStar(Vector2.zero),
                
                //CreateGameStage(10)
                //    .AddStaticBasket(BasketSide.Left, 2.0f)
                //    .AddStaticBasket(BasketSide.Left, 0.0f),
                
                //CreateGameStage(12)
                //    .AddStaticBasket(BasketSide.Left, 0.0f),
                
                //CreateGameStage(13)
                //    .AddDynamicBasket(BasketSide.Left,0.0f, 3.5f, 4.0f, Ease.Linear),
                //SET y<0 = 0
                CreateGameStage(0).AddStaticBasket(BasketSide.Left, 0.915f),
                CreateGameStage(1).AddStaticBasket(BasketSide.Left, 0f),
                CreateGameStage(2).AddStaticBasket(BasketSide.Left, 0f).AddStar(Vector2.zero),
                CreateGameStage(5).AddDynamicBasket(BasketSide.Left, 0f, 1.5f, 3.0f, Ease.Linear),
                CreateGameStage(6).AddStaticBasket(BasketSide.Left,3.0f).AddStar(Vector2.one),
                CreateGameStage(7)
                    .AddDynamicBasket(BasketSide.Left,3.0f, .0f, 4.0f, Ease.Linear),

                CreateGameStage(8)
                    .AddStaticBasket(BasketSide.Left, 4.0f)
                    .AddStaticBasket(BasketSide.Left, 1.0f)
                    .AddStar(Vector2.zero),

                CreateGameStage(10)
                    .AddStaticBasket(BasketSide.Left, 2.0f)
                    .AddStaticBasket(BasketSide.Left, 0.0f),

                CreateGameStage(12)
                    .AddStaticBasket(BasketSide.Left, 0.0f),

                CreateGameStage(13)
                    .AddDynamicBasket(BasketSide.Left,0.0f, 3.5f, 4.0f, Ease.Linear),


            };

        }

        private void ResetAllIfNeed()
        {
            if (_resourceService.GetResourceAmount(ResourceType.Score) >= 15)
            {
                _resourceService.SetResourceAmount(ResourceType.Score, 0);
                InitializeStages();
                Debug.Log("Reset all");
            }
        }

        private void NextStage()
        {
            ResetAllIfNeed();

            GameStage foundStage = _stages.FindLast(
                stage => stage.ActivationGoalsCount <= (int)_resourceService.GetResourceAmount(ResourceType.Score));

            if (foundStage != null && foundStage != _currentStage)
            {
                _currentStage?.Release();
                _currentStage = foundStage;
                _currentStage.Activate();
            }

            _cannon.Recharge();
        }

        public void Dispose()
        {
            _eventBusService.RemoveListener(GameEventKey.GoalScored, OnGoalScored);
            _cannon.OnBallCreated.RemoveListener(OnBallCreated);
            _cannon.OnLastBallGrounded.RemoveListener(OnLevelFailed);
            _cannon.BallGrounded -= OnBallGrounded;
        }
    }
}