using System.Text;
using Libgpgme;
using PassSharp.Lib.Abstraction;
using PassSharp.Lib.Service.Abstraction;

namespace PassSharp.Lib;

public class Password : IPassword
{
    private readonly IGpgService _gpgService;
    private readonly FileInfo _fileInfo;

    public Password(IGpgService gpgService,
        string path)
    {
        _gpgService = gpgService;
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
        return await _gpgService.Decrypt(Path);
    }

    private async Task Encrypt(IEnumerable<string> data)
    {
        await _gpgService.Encrypt(Key, data, Path);
    }
}