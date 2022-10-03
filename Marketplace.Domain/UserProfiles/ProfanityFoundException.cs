namespace Marketplace.Domain.UserProfiles; 

public class ProfanityFoundException : Exception {
    public ProfanityFoundException(string text) : base($"Profanity found in text: {text}") {}
}