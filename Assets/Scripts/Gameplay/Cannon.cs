using CodeBase;
using Editor.LevelEditor;
using Management.Roots;
using Services.Audio;
using System;
using TriInspector;
using UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.PlayerLoop;
using UnityEngine.Serialization;
using UnityEngine.UI;
using Utilities;

namespace Gameplay
{
    public class Cannon : MonoBehaviour
    {
        public event Action BallGrounded;
        [SerializeField] private Vector2 _shotRelativeForce;
        [SerializeField] private Vector2 _shotRelativeForceBase;
        [SerializeField] private GameObject _pivot;
        [SerializeField] private SpriteRenderer _pivotSprite;
        [SerializeField] private SpriteRenderer _mainSprite;
        [SerializeField] private GameObject _bottomPlatform;
        [SerializeField] private Slider _forceSlider;
        [SerializeField] private HeadUpDisplay _headUpDisplay;

        [SerializeField] private float _cooldown = 1.0f;
        [SerializeField] private Ball _ballPrefab;
        [SerializeField] private BallsBuffer _ballsBuffer;

        [SerializeField] private GameplaySceneRoot _gameplaySceneRoot;

        private float _elapsedTime;
        private EventBusService _eventBusService;
        private AudioService _audioService;

        public UnityEvent<Ball> OnBallCreated;
        public UnityEvent OnLastBallGrounded;

        public GameObject Pivot
        {
            get { return _pivot; }
            set { _pivot = value; }
        }
        public int CurrentBalls => _ballsBuffer.BallsCount;

        private void Awake()
        {
            _elapsedTime = _cooldown;
        }

        public void Initialize(ThrowConfigData levelConfigData, LevelEditorSettings levelEditorSettings)
        {
            _pivotSprite.sprite = levelEditorSettings.SpriteAsset.GetSprite("Cannon", $"{levelConfigData.Cannon.IdCannonPivot}Pivot");
            _mainSprite.sprite = levelEditorSettings.SpriteAsset.GetSprite("Cannon", $"{levelConfigData.Cannon.IdCannonMain}Main"); ;
            if (levelConfigData.Cannon.Type == Environment.BasketSide.Left)
            {
                _bottomPlatform.transform.rotation = Quaternion.Euler(new Vector3(_bottomPlatform.transform.eulerAngles.x, 0, _bottomPlatform.transform.eulerAngles.z));
                transform.position = levelEditorSettings.CannonLeftPosition;
                _pivot.transform.rotation = Quaternion.Euler(new Vector3(_pivot.transform.eulerAngles.x, _pivot.transform.eulerAngles.y, -30));
                //transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, 0, transform.eulerAngles.z));
            }
            else
            {
                _bottomPlatform.transform.rotation = Quaternion.Euler(new Vector3(_bottomPlatform.transform.eulerAngles.x, 180f, _bottomPlatform.transform.eulerAngles.z));
                transform.position = levelEditorSettings.CannonRightPosition;
                _pivot.transform.rotation = Quaternion.Euler(new Vector3(_pivot.transform.eulerAngles.x, _pivot.transform.eulerAngles.y, 30));
                //transform.rotation = Quaternion.Euler(new Vector3(transform.eulerAngles.x, 180f, transform.eulerAngles.z));
            }
        }

        public void InjectDependencies(EventBusService eventBusService, AudioService audioService)
        {
            _eventBusService = eventBusService;
            _audioService = audioService;
            //Time.timeScale = 1.0f;
        }

        [Button]
        public void FireIfPossible(float percent)
        {
            if (_elapsedTime < _cooldown)
            {
                return;
            }

            if (_ballsBuffer.TryDequeue(out Ball ball))
            {
                ball.InjectDependencies(_eventBusService, _audioService, _gameplaySceneRoot);
                ball.Fire(CalculateForce(percent));

                _audioService?.PlayOneShot(AudioMetadata.ShotSoundName);
                _elapsedTime = 0.0f;

                if (_ballsBuffer.BallsCount == 0)
                {
                    ball.OnGrounded += () =>
                    {
                        BallGrounded?.Invoke();
                        if (_ballsBuffer.BallsCount == 0)
                        {
                            OnLastBallGrounded?.Invoke();
                        }
                    };
                }

                OnBallCreated?.Invoke(ball);
            }
            else
            {
                Debug.Log("Balls storage is empty");
            }
        }

        private Vector2 CalculateForce(float percent)
        {
            //return _shotRelativeForce;
            return _shotRelativeForceBase * (percent);
        }

        public void Recharge()
        {
            int missingBallsCount = _ballsBuffer.MaxSize - _ballsBuffer.BallsCount;

            for (int i = 0; i < missingBallsCount; i++)
            {
                SpawnBall();
            }
        }
        public void RechargeOneBall()
        {
            SpawnBall();
        }

        private void SpawnBall()
        {
            Ball ball = Instantiate<Ball>(_ballPrefab, _ballsBuffer.transform);

            if (!_ballsBuffer.TryEnqueue(ball))
            {
                Debug.LogWarning("Balls storage is overloaded");
            }
        }

        private void Update()
        {
            if (_headUpDisplay != null)
            {
                if (_headUpDisplay.AmountClick == 1 || _headUpDisplay.AmountClick == 2)
                    _shotRelativeForce = _shotRelativeForceBase * _forceSlider.value;
                else if (_headUpDisplay.AmountClick == 3)
                {


                }
                else
                    _shotRelativeForce = _shotRelativeForceBase;
                if (_elapsedTime < _cooldown)
                {
                    _elapsedTime += Time.deltaTime;
                }
            }

            if (_ballsBuffer.TryPeek(out Ball peekedBall))
            {
                peekedBall.FireSimulation(_shotRelativeForce);
            }
            else
            {
                TrajectoryPrediction.Instance.ClearTrajectory();
            }
        }
    }
}