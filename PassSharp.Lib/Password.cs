using System.Text;
using Libgpgme;
using PassSharp.Lib.Abstraction;

namespace PassSharp.Lib;

public class Password : IPassword
{
    private readonly FileInfo _fileInfo;

    public Password(string path)
    {
        Path = path.EndsWith(".gpg") ? path : path + ".gpg";
        _fileInfo = new FileInfo(path);
    }


    public string Path { get; set; }
    public Key Key { get; set; }

    public async Task<IEnumerable<string>> Show()
    {
        var fileContent = await Decrypt();

        return fileContent;
    }

    public async Task Edit(IEnumerable<string> data)
    {
        await Encrypt(data);
    }

    public Task Remove()
    {
        _fileInfo.Delete();
        return Task.CompletedTask;
    }

    public Task Move(string newPath)
    {
        _fileInfo.MoveTo(newPath);
        return Task.CompletedTask;
    }

    public Task Move(DirectoryInfo newPath)
    {
        return Move(newPath.FullName);
    }

    public Task Copy(string destination)
    {
        _fileInfo.CopyTo(destination);
        return Task.CompletedTask;
    }

    public Task Copy(DirectoryInfo destination)
    {
        return Copy(destination.FullName);
    }


    private async Task<string[]> Decrypt()
    {
        await using var file = new GpgmeMemoryData(Path);
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

    private async Task Encrypt(IEnumerable<string> data)
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
            Path,
            FileMode.Create,
            FileAccess.ReadWrite);

        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);

        context.Encrypt(
            new[] { Key },
            EncryptFlags.AlwaysTrust,
            plain,
            cipherfile);

        plain.Close();
        cipherfile.Close();
    }
}