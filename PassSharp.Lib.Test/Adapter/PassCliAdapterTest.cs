using System;
using System.Threading.Tasks;
using PassSharp.Lib.Adapter;
using Xunit;

namespace PassSharp.Lib.Test.Adapter;

public class PassCliAdapterTest : IDisposable
{
    public async void ShowTest()
    {
        // arrange
        var passCliAdapter = new PassCliAdapter();
        const string expectedPassword = "test";
        // act
        var result = await passCliAdapter.Show("testPassword");
        // assert
        Assert.Equal(expectedPassword, result);
    }
    
    public void Dispose()
    {
    }
}