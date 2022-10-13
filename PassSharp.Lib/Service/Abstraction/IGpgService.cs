using Libgpgme;

namespace PassSharp.Lib.Service.Abstraction;

public interface IGpgService
{
    public Key Key { get; init; }
    Task Encrypt(IEnumerable<string> data, string path);

    Task<string[]> Decrypt(string path);

    IEnumerable<Key> GetKeys(string[] gpgIds);
}