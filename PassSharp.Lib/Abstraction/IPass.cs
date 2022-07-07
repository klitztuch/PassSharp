namespace PassSharp.Lib;

public interface IPass
{
    IGit Git
    {
        get;
    } 

    string PasswordStoreLocation
    {
        get;
        init;
    }
    void Init();
    Task<IPasswordNode> List();
    IPasswordNode List(string subfolder);
    IPasswordNode Find(string name);
    Task<IPassword> Show(string name);
    IPassword Show(IPasswordNode passwordNode);
    IPassword Insert(IPassword password);
    IPassword Edit(string name, IPassword password);
    IPassword Generate(string name, int? length = null);
    void Remove(string name, bool force = false);
    void Remove(IPassword password, bool force = false);
    void Move(string oldPath, string newPath, bool force = false);
    void Copy(string oldPath, string newPath, bool force = false);
    string Version();
}