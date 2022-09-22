using LibGit2Sharp;
using Libgpgme;
using PassSharp.Lib.Abstraction;

namespace PassSharp.Lib;

public class Pass : IPass
{
    public Pass(IRepository repository)
    {
        Repository = repository;
        PasswordStoreLocation ??=
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".password-store");
        if (Key is null)
        {
            throw new Exception("Key not set");
        }
    }

    public IRepository Repository { get; set; }
    public string PasswordStoreLocation { get; init; }
    public Key Key { get; init; }

    public void Init()
    {
        if (!Directory.Exists(PasswordStoreLocation))
            Directory.CreateDirectory(PasswordStoreLocation);
        else
            throw new Exception("PasswordStore already exists");
        
        // create gpgId File
        var gpgIdPath = Path.Combine(PasswordStoreLocation, ".gpg-id");
        File.Create(gpgIdPath);
        var streamWriter = new StreamWriter(gpgIdPath);
        streamWriter.Write(Key.KeyId);
        streamWriter.Flush();
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
            .Select(o => new Password(o.FullName));
    }

    public IEnumerable<IPassword> Find(string name)
    {
        var passwordStore = new DirectoryInfo(PasswordStoreLocation);
        var passwords = passwordStore.GetFiles(name, SearchOption.AllDirectories)
            .Select(o => new Password(o.FullName));
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