namespace Services.Data.Crypto
{
    public interface IStreamCryptoService
    {
        public string Encrypt(string data);
        public string Decrypt(string data);
    }
}