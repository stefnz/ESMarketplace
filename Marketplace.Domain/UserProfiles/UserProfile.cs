using ES.Framework;

namespace Marketplace.Domain.UserProfiles; 

public class UserProfile: Aggregate<UserId> {
    // Required by RavenDb to set the Id of the document
    private string DbId {
        get => $"UserProfile/{Id.Value}";
        set {}
    }

    public override UserId Id { get; protected set; }
    public FullName FullName { get; private set; }
    public DisplayName DisplayName { get; private set; }
    public string PhotoUrl { get; private set; }

    private UserProfile() : base() { }

    public UserProfile(UserId id, FullName fullName, DisplayName displayName) =>
        Apply(new UserProfileEvents.UserRegistered {
            UserId = id,
            DisplayName = displayName,
            FullName = fullName
        });

    public void UpdateFullName(FullName fullName) =>
        Apply(new UserProfileEvents.UserFullnameUpdated {
            UserId = Id,
            Fullname = fullName
        });

    public void UpdateDisplayName(DisplayName displayName) =>
        Apply(new UserProfileEvents.UserDisplayNameUpdated {
            UserId = Id,
            DisplayName = displayName
        });

    public void UpdateProfilePhoto(Uri profilePhotoUri) =>
        Apply(new UserProfileEvents.ProfilePhotoUploaded {
            UserId = Id,
            PhotoUrl = profilePhotoUri.ToString()
        });
    
    
    protected override void When(object @event) {
        switch (@event) {
            case UserProfileEvents.UserRegistered e:
                Id = new UserId(e.UserId);
                FullName = new FullName(e.FullName);
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case UserProfileEvents.UserFullnameUpdated e:
                FullName = new FullName(e.Fullname);
                break;
            case UserProfileEvents.UserDisplayNameUpdated e:
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case UserProfileEvents.ProfilePhotoUploaded e:
                PhotoUrl = e.PhotoUrl;
                break;
        }
    }

    protected override void EnsureValidState() {
        // none yet
    }
}