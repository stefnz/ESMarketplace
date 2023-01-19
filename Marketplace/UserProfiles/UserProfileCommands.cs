using Marketplace.Domain.UserProfiles;

namespace Marketplace.UserProfiles; 

// Types in the contract must be standard types

public class UserProfileContract {
    public static class V1 {
        public class RegisterUser {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
            public string DisplayName { get; set; }
        }

        public class UpdateUserDisplayName {
            public Guid UserId { get; set; }
            public string DisplayName { get; set; }
        }

        public class UpdateUserFullName {
            public Guid UserId { get; set; }
            public string FullName { get; set; }
        }

        public class UpdateUserProfilePhoto {
            public Guid UserId { get; set; }
            public string ProfilePhotoUrl { get; set; }

        }
    }
}