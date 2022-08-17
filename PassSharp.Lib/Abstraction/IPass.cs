using System.Security;
using LibGit2Sharp;

namespace PassSharp.Lib.Abstraction;

public interface IPass
{
    IRepository Repository { get; set; }

    string PasswordStoreLocation { get; init; }

    void Init();
    IEnumerable<IPassword> List();
    IEnumerable<IPassword> List(string subfolder);
    IEnumerable<IPassword> Find(string name);
    Task Insert(IPassword password, IEnumerable<SecureString> data);

    Task<IPassword> Generate(IPassword password,
        int? length = null);
}