using UnityEngine;
using VisualEffects;

namespace Factories
{
    [CreateAssetMenu(fileName = "visual_effect_factory", menuName = "Factories/VisualEffectFactory", order = 0)]
    public class VisualEffectFactory : ScriptableObject
    {
        [SerializeField] private Exclamation _exclamationPrefab;
        
        public Exclamation CreateExclamation(Vector3 worldPosition, string text)
        {
            return Instantiate<Exclamation>(_exclamationPrefab, worldPosition, Quaternion.identity).Run(text);
        }
    }
}