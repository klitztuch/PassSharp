namespace PassSharp.Lib.Abstraction;

public interface IPass
{
    IGit Git { get; }

    string PasswordStoreLocation { get; init; }

    void Init();
    Task<ITreeNode<IPassword>?> List();
    Task<ITreeNode<IPassword>?> List(string subfolder);
    Task<ITreeNode<IPassword>> Find(string name);
    Task<IPassword?> Show(string name);
    Task<IPassword> Show(ITreeNode<Password> treeNode);
    Task<IPassword> Insert(IPassword password);
    Task<IPassword> Edit(string name, IPassword password);
    Task<IPassword> Generate(string name, int? length = null);
    void Remove(string name, bool force = false);
    void Remove(IPassword password, bool force = false);
    void Move(string oldPath, string newPath, bool force = false);
    void Copy(string oldPath, string newPath, bool force = false);
    string Version();
}