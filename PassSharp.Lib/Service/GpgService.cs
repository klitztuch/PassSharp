using Libgpgme;
using PassSharp.Lib.Service.Abstraction;

namespace PassSharp.Lib.Service;

public class GpgService : IGpgService
{
    public string Encrypt(string data)
    {
        throw new NotImplementedException();
    }

    public string Decrypt(string data)
    {
        throw new NotImplementedException();
    }

    public Key[] GetKeys(string[] gpgIds)
    {
        throw new NotImplementedException();
    }
}