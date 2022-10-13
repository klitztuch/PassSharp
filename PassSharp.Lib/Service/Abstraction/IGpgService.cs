using Libgpgme;

namespace PassSharp.Lib.Service.Abstraction;

public interface IGpgService
{
    
    Task Encrypt(Key key, IEnumerable<string> data, string path);

    Task<string[]> Decrypt(string path);

    Key[] GetKeys(string[] gpgIds);
}