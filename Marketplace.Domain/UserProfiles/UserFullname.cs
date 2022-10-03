using System;

namespace Marketplace.Domain.UserProfiles;

public record FullName {
    public string Value { get; }
    internal FullName(string fullName) => Value = fullName;

    public static FullName FromString(string fullName) {
        if (string.IsNullOrWhiteSpace(fullName)) {
            throw new ArgumentNullException(nameof(fullName));
        }

        return new FullName(fullName);
    }

    public static implicit operator string(FullName fullName) => fullName.Value;

    // Satisfy the serialization requirements
    protected FullName() { }
}