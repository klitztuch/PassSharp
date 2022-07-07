using Moq;
using PassSharp.Lib.Adapter.Abstraction;
using Xunit;

namespace PassSharp.Lib.Test;

public class PassTest
{
    [Fact]
    public async void ShowTest()
    {
        // arrange
        const string expectedPasswordName = "testPassword";
        var passAdapterMock = new Mock<IPassAdapter>();
        passAdapterMock.Setup(passAdapter => passAdapter.Show(expectedPasswordName).Result)
        .Returns(expectedPasswordName);
        var gitMock = new Mock<IGit>();
        Pass pass = new Pass(passAdapterMock.Object, gitMock.Object){
            PasswordStoreLocation = ".passwordstore-test/"
        };
        // act
        var result = await pass.Show(expectedPasswordName);
        // assert
        Assert.IsType<Password>(result);
    }
}