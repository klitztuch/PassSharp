using System.Security;

namespace PassSharp.Lib.Abstraction;

public interface IPassword
{
    string Path { get; set; }
    Task<IEnumerable<SecureString>> Show();
    Task Edit(IEnumerable<SecureString> data);
    Task Remove();
    Task Move(string newPath);
    Task Move(DirectoryInfo newPath);
    Task Copy(string destination);
    Task Copy(DirectoryInfo destination);
    
}
