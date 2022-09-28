using System;
using Libgpgme;

namespace PassSharp.Lib.Test;

public class GpgFixture : IDisposable
{
    public GpgFixture()
    {
        // create test gpg key
        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);

        var keyParameter = new KeyParameters
        {
            RealName = "PassSharpTestKey",
            Email = "PassSharpTestKey@home.internal",
            ExpirationDate = DateTime.Now.AddYears(2),
            Passphrase = "topsecret"
        };
        var keyResult = context.KeyStore.GenerateKey(Protocol.OpenPGP, keyParameter);
        Key = context.KeyStore.GetKey(keyResult.Fingerprint, true);
    }

    public Key Key { get; }

    public void Dispose()
    {
        // remove test gpg key
        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);
        context.KeyStore.DeleteKey(Key, true);
    }
}