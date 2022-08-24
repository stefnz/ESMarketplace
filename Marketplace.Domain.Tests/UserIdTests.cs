namespace Marketplace.Domain.Tests;

public class UserIdTests
{
    [Fact]
    public void Can_create_UserId_with_Guid()
    {
        var guid = Guid.NewGuid();
        var userId = new UserId(guid);
        Assert.Equal(guid, userId.Value);
    }

    [Fact]
    public void UserId_cannot_use_default_guid()
    {
        Assert.Throws<ArgumentNullException>(() => new UserId(default));
    }
}