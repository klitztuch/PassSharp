using LibGit2Sharp;
using Libgpgme;
using PassSharp.Lib.Abstraction;
using PassSharp.Lib.Service.Abstraction;

namespace PassSharp.Lib;

public class Pass : IPass
{
    private readonly IGpgService _gpgService;

    public Pass(IRepository repository, IGpgService gpgService)
    {
        _gpgService = gpgService;
        Repository = repository;
        PasswordStoreLocation ??=
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".password-store");
    }

    public IRepository Repository { get; set; }
    public string PasswordStoreLocation { get; init; }

    public async Task Init(string[] gpgIds,
        string? subPath = null)
    {
        if (!Directory.Exists(PasswordStoreLocation))
        {
            Directory.CreateDirectory(PasswordStoreLocation);
        }

        // create gpgId File
        var gpgIdPath = string.IsNullOrEmpty(subPath)
            ? Path.Combine(PasswordStoreLocation, ".gpg-id")
            : Path.Combine(PasswordStoreLocation, subPath, ".gpg-id");

        File.Create(gpgIdPath);
        var streamWriter = new StreamWriter(gpgIdPath);

        var keys = _gpgService.GetKeys(gpgIds);
        foreach (var key in keys)
        {
            await streamWriter.WriteLineAsync(key.Fingerprint);
        }
        await streamWriter.FlushAsync();
        streamWriter.Close();
    }

    public IEnumerable<IPassword> List()
    {
        return List("*");
    }

    public IEnumerable<IPassword> List(string subfolder)
    {
        var directory = new DirectoryInfo(PasswordStoreLocation);
        return directory.GetFiles("*", SearchOption.AllDirectories)
            .Select(o => new Password(_gpgService, o.FullName));
    }

    public IEnumerable<IPassword> Find(string name)
    {
        var passwordStore = new DirectoryInfo(PasswordStoreLocation);
        var passwords = passwordStore.GetFiles(name, SearchOption.AllDirectories)
            .Select(o => new Password(_gpgService, o.FullName));
        return passwords;
    }

    public IEnumerable<IPassword> FuzzyFind(string name)
    {
        return Find($"*{name}*");
    }

    public async Task Insert(IPassword password,
        IEnumerable<string> data)
    {
        File.Create(password.Path).Close();
        await password.Edit(data);
    }

    public Task<IPassword> Generate(IPassword password,
        int? length = null)
    {
        throw new NotImplementedException();
    }
}