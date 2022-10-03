namespace Marketplace.Domain.UserProfiles; 

public interface IUserProfileRepository {
    Task<UserProfile> Load(UserId id);

    Task Add(UserProfile userProfile);
    Task<bool> Exists(UserId id);
}