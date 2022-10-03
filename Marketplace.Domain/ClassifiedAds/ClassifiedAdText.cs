namespace Marketplace.Domain; 

public record ClassifiedAdText {
    public string Value { get; }
    protected ClassifiedAdText() { }
    internal ClassifiedAdText(string text) => Value = text;
        
    public static ClassifiedAdText FromString(string text) => new ClassifiedAdText(text);
    public static implicit operator string(ClassifiedAdText text) => text.Value;
}
