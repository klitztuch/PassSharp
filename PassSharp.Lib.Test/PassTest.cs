using Xunit;

namespace PassSharp.Lib.Test;

public class PassTest
{
    [Fact]
    public async void InitTest()
    {
        // arrange
        Pass pass = new Pass();
        // act
        var result = await pass.List();
        // assert
        Assert.Null(result);
    }
}