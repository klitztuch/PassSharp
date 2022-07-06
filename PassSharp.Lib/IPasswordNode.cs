namespace PassSharp.Lib;

public interface IPasswordNode
{
    IPassword Password
    {
        get;
        set;
    }

    List<IPasswordNode> Children
    {
        get;
    }

    bool HasChildren
    {
        get;
    }

    void AddChild(IPasswordNode node);
    void AddChild(IPassword password);
}