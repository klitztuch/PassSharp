using LibGit2Sharp;
using Libgpgme;

namespace PassSharp.Lib.Abstraction;

public interface IPass
{
    IRepository Repository { get; set; }

    string PasswordStoreLocation { get; init; }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="gpgIds"></param>
    /// <param name="subPath"></param>
    /// <returns></returns>
    Task Init(string[] gpgIds,
        string? subPath = null);
    IEnumerable<IPassword> List();
    IEnumerable<IPassword> List(string subfolder);
    IEnumerable<IPassword> Find(string name);
    IEnumerable<IPassword> FuzzyFind(string name);

    Task Insert(IPassword password,
        IEnumerable<string> data);

    Task<IPassword> Generate(IPassword password,
        int? length = null);
}