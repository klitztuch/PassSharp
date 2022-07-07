using System.Text;
using CliWrap;
using PassSharp.Lib.Adapter;
using PassSharp.Lib.Adapter.Abstraction;

namespace PassSharp.Lib;

public class Pass : IPass
{
    private IPassAdapter _passAdapter { get; set; }
    public IGit Git { get; }
    public string PasswordStoreLocation { get; init; }

    public Pass(IPassAdapter passAdapter, IGit git)
    {
        _passAdapter = passAdapter;
        Git = git;
    }

    public void Init()
    {
        throw new NotImplementedException();
    }

    public async Task<IPasswordNode> List()
    {
        var result = new StringBuilder();
        var t = await Cli.Wrap("pass")
            .WithArguments("ls")
            .WithStandardOutputPipe(PipeTarget.ToStringBuilder(result))
            .ExecuteAsync();
        return null;
    }

    public IPasswordNode List(string subfolder)
    {
        throw new NotImplementedException();
    }

    public IPasswordNode Find(string name)
    {
        throw new NotImplementedException();
    }

    public async Task<IPassword> Show(string name)
    {
        var password = await _passAdapter.Show(name);
        return new Password();
    }

    public IPassword Show(IPasswordNode passwordNode)
    {
        throw new NotImplementedException();
    }

    public IPassword Insert(IPassword password)
    {
        throw new NotImplementedException();
    }

    public IPassword Edit(string name, IPassword password)
    {
        throw new NotImplementedException();
    }

    public IPassword Generate(string name, int? length = null)
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
}