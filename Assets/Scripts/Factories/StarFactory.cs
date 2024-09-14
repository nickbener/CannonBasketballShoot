using Gameplay.Environment;
using ResourceSystem;
using UnityEngine;

namespace Factories
{
    [CreateAssetMenu(fileName = "star_factory", menuName = "Factories/StarFactory", order = 0)]
    public class StarFactory : ScriptableObject
    {
        [SerializeField] private Star _starPrefab;

        private ResourceSystemService _resourceService;
        
        public StarFactory Initialize(ResourceSystemService resourceService)
        {
            _resourceService = resourceService;
            return this;
        }

        public Star CreateStar(Vector2 position)
        {
            return Instantiate(_starPrefab, position, Quaternion.identity).Initialize(_resourceService);
        }

    }
}