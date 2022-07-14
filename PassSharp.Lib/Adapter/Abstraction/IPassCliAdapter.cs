namespace PassSharp.Lib.Adapter.Abstraction;

public interface IPassCliAdapter
{
    Task<string> Show(string name);
}
