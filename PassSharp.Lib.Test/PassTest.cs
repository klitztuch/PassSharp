using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LibGit2Sharp;
using Moq;
using Newtonsoft.Json;
using PassSharp.Lib.Abstraction;
using Xunit;

namespace PassSharp.Lib.Test;

public class PassTest : IDisposable
{
    private const string TEST_BASE_DIRECTORY = "test-files";
    private const int TEST_FILES_COUNT = 10;
    private readonly string _passwordStoreDirectory = Path.Combine(TEST_BASE_DIRECTORY, "password-store-test");

    public PassTest()
    {
        Directory.CreateDirectory(_passwordStoreDirectory);
        foreach (var i in Enumerable.Range(0, TEST_FILES_COUNT)) File.Create($"{_passwordStoreDirectory}/first{i}.gpg");

        Directory.CreateDirectory($"{_passwordStoreDirectory}/subDirectory");
        foreach (var i in Enumerable.Range(0, TEST_FILES_COUNT))
            File.Create($"{_passwordStoreDirectory}/subDirectory/second{i}.gpg");
    }

    public void Dispose()
    {
        Directory.Delete(TEST_BASE_DIRECTORY, true);
    }


    [Fact]
    public void ListTest()
    {
        // arrange
        const int expectedFileCount = TEST_FILES_COUNT * 2;
        var repositoryMock = new Mock<IRepository>();
        var pass = new Pass(repositoryMock.Object)
        {
            PasswordStoreLocation = _passwordStoreDirectory
        };
        // act
        var result = pass.List();
        // assert
        Assert.IsAssignableFrom<IEnumerable<IPassword>>(result);
        Assert.Equal(expectedFileCount, result.Count());
    }

    [Fact]
    public void ListNoDirectoryTest()
    {
        // arrange
        var repositoryMock = new Mock<IRepository>();
        var pass = new Pass(repositoryMock.Object)
        {
            PasswordStoreLocation = Path.Combine(_passwordStoreDirectory, "-wrong")
        };

        // act
        IEnumerable<IPassword> Result()
        {
            return pass.List();
        }

        // assert
        Assert.Throws<DirectoryNotFoundException>((Func<IEnumerable<IPassword>>)Result);
    }

    [Fact]
    public void InitTest()
    {
        // arrange
        var repositoryMock = new Mock<IRepository>();
        var pass = new Pass(repositoryMock.Object)
        {
            PasswordStoreLocation = Path.Combine(_passwordStoreDirectory, "-new")
        };

        // act
        pass.Init();

        // assert
        Assert.True(Directory.Exists(_passwordStoreDirectory));
    }

    [Fact]
    public void InitExistingTest()
    {
        // arrange
        var repositoryMock = new Mock<IRepository>();
        var pass = new Pass(repositoryMock.Object)
        {
            PasswordStoreLocation = _passwordStoreDirectory
        };

        // act
        void Result()
        {
            pass.Init();
        }

        // assert
        Assert.Throws<Exception>(Result);
    }

    [Fact]
    public void FindTest()
    {
        // arrange
        var repositoryMock = new Mock<IRepository>();
        var pass = new Pass(repositoryMock.Object)
        {
            PasswordStoreLocation = _passwordStoreDirectory
        };

        const string expectedFilename = "first1.gpg";
        IPassword expectedPassword = new Password(Path.Combine(Environment.CurrentDirectory, _passwordStoreDirectory, expectedFilename));
        IEnumerable<IPassword> expectedResult = new List<IPassword>()
        {
            expectedPassword
        };

        // act
        var actualResult = pass.Find(expectedFilename);
        
        // assert
        Assert.Equal(JsonConvert.SerializeObject(expectedResult),
            JsonConvert.SerializeObject(actualResult));
    }
    
    [Fact]
    public void FuzzyFindTest()
    {
        // arrange
        var repositoryMock = new Mock<IRepository>();
        var pass = new Pass(repositoryMock.Object)
        {
            PasswordStoreLocation = _passwordStoreDirectory
        };

        const string expectedFilename = "first1";
        IPassword expectedPassword = new Password(Path.Combine(Environment.CurrentDirectory, _passwordStoreDirectory, expectedFilename));
        IEnumerable<IPassword> expectedResult = new List<IPassword>()
        {
            expectedPassword
        };

        // act
        var actualResult = pass.FuzzyFind(expectedFilename);
        
        // assert
        Assert.Equal(JsonConvert.SerializeObject(expectedResult),
            JsonConvert.SerializeObject(actualResult));
    }

    [Fact]
    public async void InsertTest()
    {
        // arrange
        var repositoryMock = new Mock<IRepository>();
        var pass = new Pass(repositoryMock.Object)
        {
            PasswordStoreLocation = _passwordStoreDirectory
        };

        const string expectedFilename = "my-new-password-file.gpg";
        const string expectedPasswordValue = "my-test-password";
        IPassword expectedPassword = new Password(Path.Combine(Environment.CurrentDirectory, _passwordStoreDirectory, expectedFilename));
        IEnumerable<IPassword> expectedResult = new List<IPassword>()
        {
            expectedPassword
        };

        // act
        await pass.Insert(expectedPassword, new List<string>()
        {
            expectedPasswordValue
        });
        
        // assert
        Assert.True(File.Exists(expectedPassword.Path));
    }
}