namespace Marketplace.Domain; 

public record ClassifiedAdTitle {
    public string Value { get; }
    private ClassifiedAdTitle(string value) => Value = value;
    private static void CheckValidity(string title) {
        if (title.Length > 100) {
            throw new ArgumentOutOfRangeException(nameof(title), "Title cannot be longer than 100 characters.");
        }
    }

    public static ClassifiedAdTitle FromString(string title) {
        CheckValidity(title);
        return new ClassifiedAdTitle(title);        
    }
    
    public static implicit operator string(ClassifiedAdTitle title) => title.Value;
}