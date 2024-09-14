using System;
using CodeBase;
using Gameplay.Environment;
using Management.Roots;
using Services.Audio;
using UnityEngine;
using UnityEngine.SceneManagement;
using Utilities;

namespace Gameplay
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Ball : MonoBehaviour
    {
        [SerializeField] private Rigidbody2D _rigidbody;
        [SerializeField] private Collider2D _collider2d;
        [SerializeField] private SphereCollider _sphereCollider;
        [SerializeField] private ParticleSystem _trail;

        [SerializeField] private Rigidbody2D _simulatorPrefab;

        public bool isLearnSceneEnable = false;

        private GameplaySceneRoot _gameplaySceneRoot;
        private EventBusService _eventBusService;
        private AudioService _audioService;
        private bool _isFirstBasket = true;

        private bool _isGrounded;
        public ClothSphereColliderPair SphereColliderPair => new ClothSphereColliderPair(_sphereCollider);
        public event Action OnGrounded;
        
        private void Awake()
        {
            _collider2d.enabled = false;
        }

        public void InjectDependencies(EventBusService eventBusService, AudioService audioService, GameplaySceneRoot gameplaySceneRoot)
        {
            _gameplaySceneRoot = gameplaySceneRoot;
            _eventBusService = eventBusService;
            _audioService = audioService;
        }

        public void FireSimulation(Vector2 relativeForce)
        {
            Rigidbody2D simulator = Instantiate(_simulatorPrefab, transform.position, transform.rotation);
            simulator.transform.localScale = transform.lossyScale;
            
            TrajectoryPrediction.Instance.MoveToSimulationScene(simulator.gameObject);
            
            simulator.bodyType = RigidbodyType2D.Dynamic;
            simulator.AddRelativeForce(relativeForce);
            
            TrajectoryPrediction.Instance.SimulateTrajectory(simulator);
            
        }
        
        public void Fire(Vector2 relativeForce)
        {
            transform.parent = null;
            
            _collider2d.enabled = true;
            _rigidbody.bodyType = RigidbodyType2D.Dynamic;
            _rigidbody.AddRelativeForce(relativeForce);
            
            _trail.Play(true);
        }

        public void DestroyAfterAnim()
        {
            Destroy(gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isFirstBasket)
                return;
            if (other.TryGetComponent<Star>(out Star star))
            {
                star.Collect();
                _gameplaySceneRoot.AmountStar += 1;
            }
            if (other.CompareTag("Spike"))
            {
                _trail.Stop(true);
                gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
                gameObject.GetComponent<Animator>().SetTrigger("Boom");
                _isGrounded = true;
                OnGrounded?.Invoke();
                return;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!_isFirstBasket)
                return;
            if (isLearnSceneEnable == false)
            {
                if (other.TryGetComponent<Basket>(out Basket basket))
                {
                    _eventBusService.Broadcast(GameEventKey.GoalScored, this);
                    _audioService?.PlayOneShot(AudioMetadata.GoalSoundName);
                    _isFirstBasket = false;
                    Debug.Log("AAA");

                    //
                    _trail.Stop(true);
                    Destroy(gameObject, 0.5f);
                    _isGrounded = true;
                }
            }
            else if (isLearnSceneEnable)
            {
                SceneManager.LoadScene(2);
            }
            

        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (!_isGrounded && other.gameObject.TryGetComponent<Border>(out Border border) 
                             && border.Type == BorderType.Bottom)
            {
                _trail.Stop(true);
                Destroy(gameObject, 3.0f);
                _isGrounded = true;
                
                OnGrounded?.Invoke();
            }
        }
    }
}