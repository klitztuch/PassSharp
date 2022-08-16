using System.Security;
using Libgpgme;
using PassSharp.Lib.Abstraction;

namespace PassSharp.Lib;

public class Password : IPassword
{
    private FileInfo _fileInfo;
    public Password(string path)
    {
        Path = path;
        _fileInfo = new FileInfo(path);
    }


    public string Path { get; set; }

    public async Task<IEnumerable<SecureString>> Show()
    {
        using var memoryStream = OpenMemoryStream();
        using var reader = new StreamReader(memoryStream);
        var content = new List<SecureString>();
        var fileContent = (await reader.ReadToEndAsync()).Split(Environment.NewLine);

        foreach (var pair in fileContent)
        {
            var secureString = new SecureString();
            foreach (var chars in pair) secureString.AppendChar(chars);
            content.Add(secureString);
        }

        return content;
    }

    public async Task Edit(IEnumerable<SecureString> data)
    {
        using var memoryStream = OpenMemoryStream();
        await using var writer = new StreamWriter(memoryStream);
        foreach (var secureString in data)
        {
            await writer.WriteLineAsync(secureString.ToString());
        }
        await writer.FlushAsync();
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
        throw new NotImplementedException();
    }

    public Task Copy(DirectoryInfo destination)
    {
        throw new NotImplementedException();
    }

    private MemoryStream OpenMemoryStream()
    {
        var file = new GpgmeMemoryData(Path);
        var memoryStream = new MemoryStream();
        var streamData = new GpgmeStreamData(memoryStream);

        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);

        var dest = context.Decrypt(file, streamData);
        if (dest == null)
        {
            throw new DecryptionFailedException("Decryption failed");
        }

        memoryStream.Position = 0;
        file.Close();
        return memoryStream;
    }
}