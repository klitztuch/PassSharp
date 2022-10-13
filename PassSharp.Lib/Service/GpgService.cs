using System.Text;
using Libgpgme;
using PassSharp.Lib.Service.Abstraction;

namespace PassSharp.Lib.Service;

public sealed class GpgService : IGpgService
{
    private static readonly Lazy<GpgService> Lazy = new Lazy<GpgService>(() => new GpgService());

    public static GpgService Instance
    {
        get { return Lazy.Value; }
    }

    private GpgService()
    {
        
    }
    public async Task Encrypt(Key key, IEnumerable<string> data, string path)
    {
        var utf8 = new UTF8Encoding();
        GpgmeData plain = new GpgmeMemoryData();
        // TODO: is filename used?
        plain.FileName = "my_document.txt";

        var streamWriter = new StreamWriter(plain, utf8);
        foreach (var line in data)
        {
            await streamWriter.WriteLineAsync(line);
        }

        await streamWriter.FlushAsync();

        GpgmeData cipherfile = new GpgmeFileData(
            path,
            FileMode.Create,
            FileAccess.ReadWrite);

        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);

        context.Encrypt(
            new[] { key },
            EncryptFlags.AlwaysTrust,
            plain,
            cipherfile);

        plain.Close();
        cipherfile.Close();
    }

    public async Task<string[]> Decrypt(string path)
    {
        await using var file = new GpgmeMemoryData(path);
        using var memoryStream = new MemoryStream();
        var streamData = new GpgmeStreamData(memoryStream);

        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);

        var dest = context.Decrypt(file, streamData);
        // close file after decryption
        file.Close();
        if (dest == null)
        {
            throw new DecryptionFailedException("Decryption failed");
        }

        memoryStream.Position = 0;
        using var reader = new StreamReader(memoryStream);
        var fileContent = (await reader.ReadToEndAsync()).Split(Environment.NewLine);
        return fileContent;
    }

    public Key[] GetKeys(string[] gpgIds)
    {
        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);
        var keys = context.KeyStore.GetKeyList(gpgIds, false);
        if (keys.Length == 0)
        {
            throw new Exception("No key found");
        }

        return keys;
    }
}