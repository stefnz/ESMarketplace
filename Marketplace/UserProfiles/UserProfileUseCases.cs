using ES.Framework;
using Marketplace.Domain.ContentModeration;
using Marketplace.Domain.UserProfiles;

namespace Marketplace.UserProfiles; 

public class UserProfileUseCases: IUseCases {

    private readonly IUserProfileRepository userProfileRepository;
    private readonly IUnitOfWork unitOfWork;
    private readonly CheckForProfanity checkForProfanityAction;

    public UserProfileUseCases(
        IUserProfileRepository repository,
        IUnitOfWork unitOfWork,
        CheckForProfanity checkForProfanity
    ) {
        this.userProfileRepository = repository;
        this.unitOfWork = unitOfWork;
        this.checkForProfanityAction = checkForProfanity;
    }


    public async Task Handle(object command) {
        switch (command) {
            case UserProfileContract.V1.RegisterUser cmd:
                if (await userProfileRepository.Exists(new UserId(cmd.UserId))) {
                    throw new InvalidOperationException($"User profile with Id {cmd.UserId} already exists.");
                }

                var userProfile = new Domain.UserProfiles.UserProfile(
                    new UserId(cmd.UserId),
                    FullName.FromString(cmd.FullName),
                    DisplayName.FromString(cmd.DisplayName, checkForProfanityAction));

                await userProfileRepository.Add(userProfile);
                await unitOfWork.Commit();
                break;
            
            case UserProfileContract.V1.UpdateUserFullName cmd:
                await HandleUpdate(cmd.UserId, profile => profile.UpdateFullName(FullName.FromString(cmd.FullName)));
                break;

            case UserProfileContract.V1.UpdateUserDisplayName cmd:
                await HandleUpdate(cmd.UserId,
                    profile => profile.UpdateDisplayName(DisplayName.FromString(cmd.DisplayName, checkForProfanityAction)));
                break;

            case UserProfileContract.V1.UpdateUserProfilePhoto cmd:
                await HandleUpdate(cmd.UserId, profile => profile.UpdateProfilePhoto(new Uri(cmd.ProfilePhotoUrl)));
                break;
            
            default:
                throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown.");
        }
    }

    private async Task HandleUpdate(Guid userProfileId, Action<UserProfile> operation) {
        var userProfile = await userProfileRepository.Load(new UserId(userProfileId));

        if (userProfile == null) {
            throw new InvalidOperationException($"User profile with Id {userProfileId} cannot be found.");
        }
        operation(userProfile);
        await unitOfWork.Commit();
    }
}