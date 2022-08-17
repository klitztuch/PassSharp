using System.Security;
using LibGit2Sharp;
using PassSharp.Lib.Abstraction;

namespace PassSharp.Lib;

public class Pass : IPass
{
    public Pass(IRepository repository)
    {
        Repository = repository;
        PasswordStoreLocation ??=
            Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), ".password-store");
    }

    public IRepository Repository { get; set; }
    public string PasswordStoreLocation { get; init; }

    public void Init()
    {
        if (!Directory.Exists(PasswordStoreLocation))
        {
            Directory.CreateDirectory(PasswordStoreLocation);
        }
        else
        {
            throw new Exception("PasswordStore already exists");
        }
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


    public Task<IPassword> Insert(IPassword password)
    {
        throw new NotImplementedException();
    }

    public Task<IPassword> Generate(string name,
        int? length = null)
    {
        throw new NotImplementedException();
    }


    public async Task<IPassword> Edit(IPassword password,
        IEnumerable<SecureString> data)
    {
        await password.Edit(data);
        return password;
    }
}