using System.Security;
using PassSharp.Lib.Abstraction;
using PassSharp.Lib.Adapter.Abstraction;

namespace PassSharp.Lib;

public class Pass : IPass
{
    private readonly IPassCliAdapter _passCliAdapter;

    public Pass(IPassCliAdapter passCliAdapter,
        IGit git)
    {
        _passCliAdapter = passCliAdapter;
        Git = git;
    }

    public IGit Git { get; }
    public string PasswordStoreLocation { get; init; }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<IPassword> List()
    {
        return List("*");
    }

    public IEnumerable<IPassword> List(string subfolder)
    {
        var directory = new DirectoryInfo(PasswordStoreLocation);
        return directory.GetFileSystemInfos("*", SearchOption.AllDirectories)
            .Select(o => new Password(o.FullName));
    }

    public IEnumerable<IPassword> Find(string name)
    {
        var passwordStore = new DirectoryInfo(PasswordStoreLocation);
        var passwords = passwordStore.GetFiles(name, SearchOption.AllDirectories)
            .Select(o => new Password(o.FullName));
        return passwords;
    }


    public async Task<IEnumerable<SecureString>> Show(string name)
    {
        var passwords = Find(name);

        var password = passwords.FirstOrDefault();
        if (password is null) return Array.Empty<SecureString>();
        return await Show(password);
    }

    public async Task<IEnumerable<SecureString>> Show(IPassword password)
    {
        return await password.Show();
    }

    public Task<IPassword> Insert(IPassword password)
    {
        throw new NotImplementedException();
    }

    public async Task<IPassword> Edit(IPassword password, IEnumerable<SecureString> data)
    {
        await password.Edit(data);
        return password;
    }

    public Task<IPassword> Generate(string name,
        int? length = null)
    {
        throw new NotImplementedException();
    }

    public void Remove(string name,
        bool force = false)
    {
        throw new NotImplementedException();
    }

    public void Remove(IPassword password,
        bool force = false)
    {
        throw new NotImplementedException();
    }

    public void Move(string oldPath,
        string newPath,
        bool force = false)
    {
        throw new NotImplementedException();
    }

    public void Copy(string oldPath,
        string newPath,
        bool force = false)
    {
        throw new NotImplementedException();
    }

    public string Version()
    {
        throw new NotImplementedException();
    }
}