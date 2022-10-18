using System;
using System.Collections.Generic;
using System.Linq;
using Moq;
using PassSharp.Lib.Service.Abstraction;
using Xunit;

namespace PassSharp.Lib.Test;

public class PasswordTest : IDisposable
{
    public void Dispose()
    {
    }

    [Fact]
    public async void ShowTest()
    {
        // arrange
        const string passwordPath = "testpath";
        const string expectedPassword = "test1";
        const string expectedInformation = "login: test";
        IEnumerable<string> expectedResult = new List<string>
        {
            expectedPassword,
            expectedInformation
        };
        var gpgServiceMock = new Mock<IGpgService>();
        gpgServiceMock.Setup(service => service.Decrypt(It.IsAny<string>()))
            .ReturnsAsync((string path) => expectedResult.ToArray());


        var pass = new Password(gpgServiceMock.Object, passwordPath);
        // act
        var actualResult = await pass.Show();
        // assert
        gpgServiceMock.Verify(service => service.Decrypt(It.IsAny<string>()), Times.Once);
        Assert.IsAssignableFrom<IEnumerable<string>>(actualResult);
        Assert.Equal(expectedResult.Count(), actualResult.Count());
        Assert.Collection(actualResult, actualPassword => Assert.Equal(expectedPassword, actualPassword),
            actualInformation => Assert.Equal(expectedInformation, actualInformation));
    }
}