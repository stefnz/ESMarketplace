using ES.Framework;

namespace Marketplace.Domain.UserProfiles;

public record UserId: IAggregateId {
    public Guid Value { get; private set; }

    public UserId(Guid id) {
        if (id == default) {
            throw new ArgumentException(nameof(id), "User Id cannot be empty.");
        }

        Value = id;
    }

    public static implicit operator Guid(UserId self) => self.Value;
    
    public override string ToString() => Value.ToString();
}