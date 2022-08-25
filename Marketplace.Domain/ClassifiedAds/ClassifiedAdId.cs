namespace Marketplace.Domain;

public record ClassifiedAdId {
    public Guid Value { get; }

    public ClassifiedAdId(Guid value) {
        if (value == default) {
            throw new ArgumentNullException(nameof(value), "Classified Ad id cannot be empty");
        }

        Value = value;
    }

    public override string ToString() => Value.ToString();
    public static implicit operator Guid(ClassifiedAdId self) => self.Value;
    public static implicit operator ClassifiedAdId(string value) => new ClassifiedAdId(Guid.Parse(value));
}