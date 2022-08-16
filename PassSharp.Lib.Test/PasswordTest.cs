using System;
using System.Collections.Generic;
using System.Net;
using System.Security;
using Xunit;

namespace PassSharp.Lib.Test;

public class PasswordTest : IDisposable
{
    public void Dispose()
    {
    }

    [Fact(Skip = "Draft")]
    public async void ShowTest()
    {
        // arrange
        const string passwordPath = "testpath";
        const string expectedPasswordString = "test";
        var expectedPasswordSecureString = new NetworkCredential("", expectedPasswordString).SecurePassword;
        IEnumerable<SecureString> expectedPassword = new List<SecureString>
        {
            expectedPasswordSecureString
        };

        var pass = new Password(passwordPath);
        // act
        var result = await pass.Show();
        // assert
        Assert.IsAssignableFrom<IEnumerable<SecureString>>(result);
    }
}