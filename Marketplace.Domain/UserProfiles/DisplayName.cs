using Marketplace.Domain.ContentModeration;

namespace Marketplace.Domain.UserProfiles;

public record DisplayName {
    public string Value { get; }
    internal DisplayName(string displayName) => Value = displayName;

    public static DisplayName FromString(string displayName, CheckForProfanity hasProfanity) {
        if (string.IsNullOrWhiteSpace(displayName)) {
            throw new ArgumentNullException(nameof(FullName));
        }

        if (hasProfanity(displayName).GetAwaiter().GetResult()) {
            throw new ProfanityFoundException(displayName);
        }

        return new DisplayName(displayName);
    }

    public static implicit operator string(DisplayName displayName) => displayName.Value;

    // Satisfy the serialization requirements
    protected DisplayName() { }
}

