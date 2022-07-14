namespace PassSharp.Lib;

public class Password : IPassword
{
    public string Path { get; set; }

    public Password(string path)
    {
        Path = path;
    }
}
