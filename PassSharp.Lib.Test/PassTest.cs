using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using Moq;
using PassSharp.Lib.Abstraction;
using PassSharp.Lib.Adapter.Abstraction;
using Xunit;
using Range = System.Range;

namespace PassSharp.Lib.Test;

public class PassTest : IDisposable
{
    private const string PASSWORD_STORE_DIRECTORY = "passwordstore-test";
    private const int TEST_FILES_COUNT = 10;

    public PassTest()
    {
        Directory.CreateDirectory(PASSWORD_STORE_DIRECTORY);
        foreach (var i in Enumerable.Range(0, TEST_FILES_COUNT))
        {
            File.Create($"{PASSWORD_STORE_DIRECTORY}/test{i}");
        }

        Directory.CreateDirectory($"{PASSWORD_STORE_DIRECTORY}/subdir");
        foreach (var i in Enumerable.Range(0, TEST_FILES_COUNT))
        {
            File.Create($"{PASSWORD_STORE_DIRECTORY}/subdir/test{i}");
        }
    }

    
    
    [Fact]
    public async void ListTest()
    {
        // arrange
        var expectedFileCount = TEST_FILES_COUNT * 2;
        var passAdapterMock = new Mock<IPassCliAdapter>();
        var gitMock = new Mock<IGit>();
        var pass = new Pass(passAdapterMock.Object, gitMock.Object){
            PasswordStoreLocation = PASSWORD_STORE_DIRECTORY
        };
        // act
        var result =  pass.List();
        // assert
        Assert.IsAssignableFrom<IEnumerable<IPassword>>(result);
        Assert.Equal(expectedFileCount, result.Count());
    }
    
    [Fact]
    public async void ListNoDirectoryTest()
    {
        // arrange
        var passAdapterMock = new Mock<IPassCliAdapter>();
        var gitMock = new Mock<IGit>();
        var pass = new Pass(passAdapterMock.Object, gitMock.Object){
            PasswordStoreLocation = PASSWORD_STORE_DIRECTORY + "-wrong"
        };
        // act
        IEnumerable<IPassword> Result() => pass.List();
        // assert
        Assert.Throws<DirectoryNotFoundException>((Func<IEnumerable<IPassword>>)Result);
    }

    public void Dispose()
    {
        Directory.Delete(PASSWORD_STORE_DIRECTORY, true);
    }
}