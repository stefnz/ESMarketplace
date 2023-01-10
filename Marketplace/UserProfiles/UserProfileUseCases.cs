using ES.Framework;
using Marketplace.Domain.ContentModeration;
using Marketplace.Domain.UserProfiles;
using Marketplace.UserProfiles;


namespace Marketplace.UserProfiles {
    public class UserProfileUseCases : IUseCases {
        private readonly IAggregateStore _store;
        private readonly CheckForProfanity _checkText;

        public UserProfileUseCases(IAggregateStore store, CheckForProfanity checkText) {
            _store = store;
            _checkText = checkText;
        }

        public Task Handle(object command) =>
            command switch {
                UserProfileContract.V1.RegisterUser cmd =>
                    HandleCreate(cmd),
                UserProfileContract.V1.UpdateUserFullName cmd =>
                    HandleUpdate(
                        cmd.UserId,
                        profile => profile.UpdateFullName(
                            FullName.FromString(cmd.FullName)
                        )
                    ),
                UserProfileContract.V1.UpdateUserDisplayName cmd =>
                    HandleUpdate(
                        cmd.UserId,
                        profile => profile.UpdateDisplayName(
                            DisplayName.FromString(
                                cmd.DisplayName,
                                _checkText
                            )
                        )
                    ),
                UserProfileContract.V1.UpdateUserProfilePhoto cmd =>
                    HandleUpdate(
                        cmd.UserId,
                        profile => profile
                            .UpdateProfilePhoto(
                                new Uri(cmd.ProfilePhotoUrl)
                            )
                    ),
                _ => Task.CompletedTask
            };

        private async Task HandleCreate(UserProfileContract.V1.RegisterUser cmd) {
            if (await _store
                    .Exists<Domain.UserProfiles.UserProfile, UserId>(
                        new UserId(cmd.UserId)
                    ))
                throw new InvalidOperationException(
                    $"Entity with id {cmd.UserId} already exists"
                );

            var userProfile = new Domain.UserProfiles.UserProfile(
                new UserId(cmd.UserId),
                FullName.FromString(cmd.FullName),
                DisplayName.FromString(cmd.DisplayName, _checkText)
            );

            await _store
                .Save<Domain.UserProfiles.UserProfile, UserId>(
                    userProfile
                );
        }

        private Task HandleUpdate(Guid id, Action<Domain.UserProfiles.UserProfile> update) =>
            this.HandleUpdate(
                _store,
                new UserId(id),
                update
            );
    }
}