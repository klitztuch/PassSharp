using System.Text;
using CliWrap;
using PassSharp.Lib.Adapter.Abstraction;

namespace PassSharp.Lib.Adapter;

public class PassAdapter : IPassAdapter
{
    public async Task<string> Show(string name)
    {
        var sb = new StringBuilder();
        var result = await Cli.Wrap("pass")
        .WithArguments("show")
        .WithArguments(name)
        .WithStandardOutputPipe(PipeTarget.ToStringBuilder(sb))
        .ExecuteAsync();
        if (result.ExitCode != 0)
        {
            throw new Exception(result.ExitCode.ToString());
        }
        return sb.ToString();
    }
}
