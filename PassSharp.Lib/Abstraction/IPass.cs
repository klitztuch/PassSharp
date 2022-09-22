using LibGit2Sharp;
using Libgpgme;

namespace PassSharp.Lib.Abstraction;

public interface IPass
{
    IRepository Repository { get; set; }

    string PasswordStoreLocation { get; init; }
    
    Key Key { get; init; }

    void Init();
    IEnumerable<IPassword> List();
    IEnumerable<IPassword> List(string subfolder);
    IEnumerable<IPassword> Find(string name);
    IEnumerable<IPassword> FuzzyFind(string name);

    Task Insert(IPassword password,
        IEnumerable<string> data);

    Task<IPassword> Generate(IPassword password,
        int? length = null);
}