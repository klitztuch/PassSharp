using Libgpgme;

namespace PassSharp.Lib.Service.Abstraction;

public interface IGpgService
{
    
    string Encrypt(string data);

    string Decrypt(string data);

    Key[] GetKeys(string[] gpgIds);
}