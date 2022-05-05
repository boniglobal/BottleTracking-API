namespace Core.Utilities.Encryption
{
    public interface IAesEncryptor
    {
        long EncryptData(long id);
        long DecryptData(long trackingId);
    }
}
