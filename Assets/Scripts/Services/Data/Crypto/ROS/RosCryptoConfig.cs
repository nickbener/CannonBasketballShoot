using UnityEngine;

namespace Services.Data.Crypto.ROS
{
    [CreateAssetMenu(fileName = "ros_crypto_config", menuName = "Crypto/RosCryptoConfig", order = 0)]
    public class RosCryptoConfig : ScriptableObject
    {
        [field:SerializeField] public bool IsEnabled { get; private set; }
        [field:SerializeField] public RosCryptoKey CryptoKey { get; private set; }
    }
}