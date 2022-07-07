namespace PassSharp.Lib.Adapter.Abstraction;

public interface IPassAdapter
{
    Task<string> Show(string name);
}
