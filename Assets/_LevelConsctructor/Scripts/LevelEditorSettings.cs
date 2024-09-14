using TriInspector;
using UnityEngine;
using UnityEngine.U2D.Animation;

namespace Editor.LevelEditor
{
    [CreateAssetMenu(fileName = "LevelConfiguratorSettings", menuName = "Custom/LevelConfiguratorSettings")]
    public class LevelEditorSettings : ScriptableObject
    {
        [Title("Cannon")]
        public GameObject CannonPrefab;
        public Vector3 CannonLeftPosition;
        public Vector3 CannonRightPosition;

        [Title("Basket")]
        public GameObject BasketFrontPrefab;
        public GameObject BasketSidePrefab;
        public GameObject PointPrefab;
        public Vector3 BasketLeftPosition;
        public Vector3 BasketRightPosition;
        public Vector3 BasketFrontPosition;

        [Title("Spike")]
        public GameObject SpikePrefab;
        public Vector3 SpikeLeftPosition;
        public Vector3 SpikeLeftRotation;
        public Vector3 SpikeRightPosition;
        public Vector3 SpikeRightRotation;
        public Vector3 SpikeTopPosition;
        public Vector3 SpikeTopRotation;

        [Title("Star")]
        public GameObject StarPrefab;

        [Title("Other")]
        public SpriteLibraryAsset SpriteAsset;
    }
}
