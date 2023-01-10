namespace Marketplace.Domain.ContentModeration; 

public delegate Task<bool> CheckForProfanity(string text);