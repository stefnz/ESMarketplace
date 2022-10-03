namespace Marketplace.Domain.UserProfiles; 

public static class UserProfileEvents {
    public class UserRegistered {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string DisplayName { get; set; }
    }

    public class ProfilePhotoUploaded {
        public Guid UserId { get; set; }
        public string PhotoUrl { get; set; }
    }

    public class UserFullnameUpdated {
        public Guid UserId { get; set; }
        public string Fullname { get; set; }
    }

    public class UserDisplayNameUpdated {
        public Guid UserId { get; set; }
        public string DisplayName { get; set; }
    }
}