using PassSharp.Lib.Abstraction;

namespace PassSharp.Lib;

public class Password : IPassword
{
    public string Path { get; set; }
    public string PasswordValue { get; set; }
    public Dictionary<string, string> Information { get; set; }

    public Password(string path)
    {
        Path = path;
    }
}
