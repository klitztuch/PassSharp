using Libgpgme;

namespace PassSharp.Lib.Abstraction;

public interface IPassword
{
    string Path { get; set; }
    public Key Key { get; set; }
    Task<IEnumerable<string>> Show();
    Task Edit(IEnumerable<string> data);
    Task Remove();
    Task Move(string newPath);
    Task Move(DirectoryInfo newPath);
    Task Copy(string destination);
    Task Copy(DirectoryInfo destination);
}