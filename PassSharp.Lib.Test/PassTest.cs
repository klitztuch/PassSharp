using System;
using System.IO;
using System.Linq;
using Moq;
using PassSharp.Lib.Abstraction;
using PassSharp.Lib.Adapter.Abstraction;
using Xunit;
using Range = System.Range;

namespace PassSharp.Lib.Test;

public class PassTest : IDisposable
{
    private string _passwordStoreDirectory = "passwordstore-test";
    private int _testFilesCount = 10;
    public PassTest()
    {
        Directory.CreateDirectory(_passwordStoreDirectory);
        foreach (var i in Enumerable.Range(0, _testFilesCount))
        {
            File.Create($"{_passwordStoreDirectory}/test{i}");
        }

        Directory.CreateDirectory($"{_passwordStoreDirectory}/subdir");
        foreach (var i in Enumerable.Range(0, _testFilesCount))
        {
            File.Create($"{_passwordStoreDirectory}/subdir/test{i}");
        }
    }

    [Fact]
    public async void ShowTest()
    {
        // arrange
        const string expectedPasswordName = "testPassword";
        var passAdapterMock = new Mock<IPassCliAdapter>();
        passAdapterMock.Setup(passAdapter => passAdapter.Show(expectedPasswordName).Result)
        .Returns(expectedPasswordName);
        var gitMock = new Mock<IGit>();
        var pass = new Pass(passAdapterMock.Object, gitMock.Object){
            PasswordStoreLocation = _passwordStoreDirectory
        };
        // act
        var result = await pass.Show(expectedPasswordName);
        // assert
        Assert.IsType<Password>(result);
    }
    
    [Fact]
    public async void ListTest()
    {
        // arrange
        var passAdapterMock = new Mock<IPassCliAdapter>();
        var gitMock = new Mock<IGit>();
        var pass = new Pass(passAdapterMock.Object, gitMock.Object){
            PasswordStoreLocation = _passwordStoreDirectory
        };
        // act
        var result = await pass.List();
        // assert
        Assert.IsAssignableFrom<ITreeNode<IPassword>>(result);
        Assert.IsType<TreeNode<IPassword>>(result);
        Assert.Equal(_testFilesCount, result.Data?.Count());
        Assert.Equal(1, result.Children?.Count());
    }

    public void Dispose()
    {
        Directory.Delete(_passwordStoreDirectory, true);
    }
}