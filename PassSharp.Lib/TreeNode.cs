using PassSharp.Lib.Abstraction;

namespace PassSharp.Lib;

public class TreeNode<T> : ITreeNode<T>
{
    public TreeNode(DirectoryInfo directory)
    {
        Directory = directory;
        Children = new List<ITreeNode<T>>();
    }


    public DirectoryInfo Directory { get; set; }
    public IEnumerable<T>? Data { get; set; }
    public ITreeNode<T>? Parent { get; set; }
    public ICollection<ITreeNode<T>> Children { get; set; }
    public bool HasChildren => Children.Any();

    public ITreeNode<T> AddChild(DirectoryInfo child)
    {
        var childNode = new TreeNode<T>(child)
        {
            Parent = this
        };
        AddChild(childNode);
        return childNode;
    }

    public ITreeNode<T> AddChild(ITreeNode<T> node)
    {
        Children.Add(node);
        return node;
    }
}