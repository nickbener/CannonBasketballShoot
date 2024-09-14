namespace Services.Data.Crypto
{
    public interface IBinaryCryptoService
    {
        public byte[] Encrypt(byte[] data);
        public byte[] Decrypt(byte[] data);
    }
}