using Libgpgme;
using PassSharp.Lib.Abstraction;
using PassSharp.Lib.Adapter.Abstraction;

namespace PassSharp.Lib;

public class Pass : IPass
{
    private readonly IPassCliAdapter _passCliCliAdapter;

    public Pass(IPassCliAdapter passCliCliAdapter, IGit git)
    {
        _passCliCliAdapter = passCliCliAdapter;
        Git = git;
    }

    public IGit Git { get; }
    public string PasswordStoreLocation { get; init; }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public async Task<ITreeNode<IPassword>?> List()
    {
        var directory = new DirectoryInfo(PasswordStoreLocation);
        return await WalkDirectoryTree(directory);
    }

    public async Task<ITreeNode<IPassword>?> List(string subfolder)
    {
        var directory = new DirectoryInfo(subfolder);
        return await WalkDirectoryTree(directory);
    }

    public async Task<ITreeNode<IPassword>> Find(string name)
    {
        throw new NotImplementedException();
    }


    public async Task<IPassword?> Show(string name)
    {
        var passwords = (await Find(name)).Data;
        if (passwords == null)
        {
            return null;
        }

        var path = passwords.FirstOrDefault()?.Path;
        var file = new GpgmeMemoryData(path);
        var mb = new MemoryStream();
        var streamData = new GpgmeStreamData(mb);

        var context = new Context();
        context.SetEngineInfo(Protocol.OpenPGP, null, null);

        var dest = context.Decrypt(file, streamData);
        
        mb.Position = 0;
        string password;
        var information = new Dictionary<string, string>();
        using (var reader = new StreamReader(mb))
        {
            var fileContent = (await reader.ReadToEndAsync()).Split(Environment.NewLine);
            password = fileContent[0];
            foreach (var pair in fileContent)
            {
                var keyValue = pair.Split(": ");
                if (keyValue.Length == 1)
                {
                    continue;
                }

                information.Add(keyValue[0], keyValue[1]);
            }
        }

        file.Close();
        mb.Close();
        return new Password(path)
        {
            Information = information,
            PasswordValue = password
        };
    }

    public Task<IPassword> Show(ITreeNode<Password> treeNode)
    {
        throw new NotImplementedException();
    }

    public Task<IPassword> Insert(IPassword password)
    {
        throw new NotImplementedException();
    }

    public Task<IPassword> Edit(string name, IPassword password)
    {
        throw new NotImplementedException();
    }

    public Task<IPassword> Generate(string name, int? length = null)
    {
        throw new NotImplementedException();
    }

    public void Remove(string name, bool force = false)
    {
        throw new NotImplementedException();
    }

    public void Remove(IPassword password, bool force = false)
    {
        throw new NotImplementedException();
    }

    public void Move(string oldPath, string newPath, bool force = false)
    {
        throw new NotImplementedException();
    }

    public void Copy(string oldPath, string newPath, bool force = false)
    {
        throw new NotImplementedException();
    }

    public string Version()
    {
        throw new NotImplementedException();
    }

    private async Task<ITreeNode<IPassword>?> WalkDirectoryTree(DirectoryInfo directory)
    {
        var root = new TreeNode<IPassword>(directory);
        FileInfo[]? files;
        try
        {
            files = directory.GetFiles();
        }
        catch (UnauthorizedAccessException e)
        {
            Console.WriteLine(e);
            return null;
        }
        catch (DirectoryNotFoundException e)
        {
            Console.WriteLine(e);
            return null;
        }

        var nodes = files.Select(file => new Password(file.FullName))
            .ToArray();

        root.Data = nodes;

        var subDirs = directory.GetDirectories();
        foreach (var subDir in subDirs)
        {
            var subNode = await WalkDirectoryTree(subDir);

            if (subNode != null) root.AddChild(subNode);
        }

        return root;
    }
}