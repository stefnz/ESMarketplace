using ES.Framework;

namespace Marketplace.Domain.UserProfiles; 

public class UserProfile: Aggregate<UserId> {
    // Required by RavenDb to set the Id of the document
    private string DbId {
        get => $"UserProfile/{Id.Value}";
        set {}
    }

    public UserId Id { get; private set; }
    public FullName FullName { get; private set; }
    public DisplayName DisplayName { get; private set; }
    public string PhotoUrl { get; private set; }

    public UserProfile(UserId id, FullName fullName, DisplayName displayName) =>
        Apply(new UserEvents.UserRegistered {
            UserId = id,
            DisplayName = displayName,
            FullName = fullName
        });

    public void UpdateFullName(FullName fullName) =>
        Apply(new UserEvents.UserFullNameUpdated {
            UserId = Id,
            FullName = fullName
        });

    public void UpdateDisplayName(DisplayName displayName) =>
        Apply(new UserEvents.UserDisplayNameUpdated {
            UserId = Id,
            DisplayName = displayName
        });

    public void UpdateProfilePhoto(Uri profilePhotoUri) =>
        Apply(new UserEvents.ProfilePhotoUploaded {
            UserId = Id,
            PhotoUrl = profilePhotoUri.ToString()
        });
    
    
    protected override void When(object @event) {
        switch (@event) {
            case UserEvents.UserRegistered e:
                Id = new UserId(e.UserId);
                FullName = new FullName(e.FullName);
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case UserEvents.UserFullNameUpdated e:
                FullName = new FullName(e.FullName);
                break;
            case UserEvents.UserDisplayNameUpdated e:
                DisplayName = new DisplayName(e.DisplayName);
                break;
            case UserEvents.ProfilePhotoUploaded e:
                PhotoUrl = e.PhotoUrl;
                break;
        }
    }

    protected override void EnsureValidState() {
        // none yet
    }

    protected UserProfile() { }
}