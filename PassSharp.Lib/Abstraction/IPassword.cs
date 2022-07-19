namespace PassSharp.Lib.Abstraction;

public interface IPassword
{
    string Path { get; set; }
    string PasswordValue { get; set; }
    Dictionary<string, string> Information { get; set; }
}