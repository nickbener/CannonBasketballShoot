using System.Text;

namespace Services.Data.Crypto.ROS
{
    public class RosCryptoService : IBinaryCryptoService, IStreamCryptoService
    {
        private readonly RosCryptoKey _cryptoKey;
        
        public RosCryptoService(RosCryptoKey cryptoKey)
        {
            _cryptoKey = cryptoKey;
        }

        public byte[] Encrypt(byte[] data)
        {
            return BinaryRosAlgorithm(data);
        }

        public byte[] Decrypt(byte[] data)
        {
            return BinaryRosAlgorithm(data);
        }

        public string Encrypt(string data)
        {
            return StreamRosAlgorithm(data);
        }

        public string Decrypt(string data)
        {
            return StreamRosAlgorithm(data);
        }

        private string StreamRosAlgorithm(string data)
        {
            return Encoding.UTF8.GetString(BinaryRosAlgorithm(Encoding.UTF8.GetBytes(data)));
        }

        private byte[] BinaryRosAlgorithm(byte[] bytes)
        {
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = (byte) RosAlgorithm(bytes[i]);
            }
            
            return bytes;
        }

        private int RosAlgorithm(int input)
        {
            return input ^ _cryptoKey.PartA ^ _cryptoKey.PartB ^ _cryptoKey.PartC ^ _cryptoKey.PartD;
        }

        
    }
}