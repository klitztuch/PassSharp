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
    Task<IPassword> Insert(IPassword password);

    Task<IPassword> Generate(string name,
        int? length = null);
}