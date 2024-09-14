using UnityEngine;
using UnityEngine.SceneManagement;

namespace Gameplay
{
    public class TrajectoryPrediction : MonoBehaviour
    {
        [SerializeField] private Transform _obstaclesParent;
        [SerializeField] private LineRenderer _trajectoryRenderer;
        [SerializeField] private int _maxPhysicsFrameIterations = 100;
        
        private Scene _scene;
        private PhysicsScene2D _physicsScene;

        public static TrajectoryPrediction Instance { get; private set; }
        
        
        private void Awake()
        {
            Instance = this;
            CreateSimulationScene();
        }
        
        private void CreateSimulationScene()
        {
            _scene = SceneManager.CreateScene("TrajectoryPredictionScene", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
            _physicsScene = _scene.GetPhysicsScene2D();

            foreach (Transform obstacle in _obstaclesParent)
            {
                GameObject clone = Instantiate(obstacle.gameObject, obstacle.position, Quaternion.identity);

                SceneManager.MoveGameObjectToScene(clone, _scene);
            }

            Destroy(_obstaclesParent.gameObject);
        }

        public void MoveToSimulationScene(GameObject go)
        {
            SceneManager.MoveGameObjectToScene(go, _scene);
        }

        public void SimulateTrajectory(Rigidbody2D projectileSimulator)
        {
            _trajectoryRenderer.positionCount = _maxPhysicsFrameIterations;
            _trajectoryRenderer.SetPosition(0, projectileSimulator.transform.position);
            
            for (int i = 1; i < _maxPhysicsFrameIterations; i++)
            {
                _physicsScene.Simulate(Time.fixedDeltaTime);
                _trajectoryRenderer.SetPosition(i, projectileSimulator.transform.position);
            }

            Destroy(projectileSimulator.gameObject);
        }

        public void ClearTrajectory()
        {
            _trajectoryRenderer.positionCount = 0;
        }
    }
}