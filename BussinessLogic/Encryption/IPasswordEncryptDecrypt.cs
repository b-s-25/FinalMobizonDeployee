namespace MobizoneApi.Models
{
    public interface IPasswordEncryptDecrypt
    {
        string Decrypt(string key, string data);
        string Encrypt(string key, string data);
    }
}