using Xunit;

namespace PassSharp.Lib.Test;

[CollectionDefinition(nameof(PassCollection))]
public class PassCollection : ICollectionFixture<GpgFixture>
{
}