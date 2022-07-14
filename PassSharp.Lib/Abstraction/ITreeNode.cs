namespace PassSharp.Lib.Abstraction;

public interface ITreeNode<T>
{
    DirectoryInfo Directory
    {
        get;
        set;
    }
    IEnumerable<T>? Data
    {
        get;
        set;
    }

    ITreeNode<T>? Parent
    {
        get;
        set;
    }

    ICollection<ITreeNode<T>> Children
    {
        get;
        set;
    }

    bool HasChildren
    {
        get;
    }

    ITreeNode<T> AddChild(DirectoryInfo child);

    ITreeNode<T> AddChild(ITreeNode<T> node);
}