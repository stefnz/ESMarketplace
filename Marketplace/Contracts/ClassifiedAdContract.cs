namespace Marketplace.Contracts; 

/// <summary>
/// DTOs for the Classified Ad resource
/// </summary>
public class ClassifiedAdContract {
    public static class V1 {
        /// <summary>
        /// Command to create a new classified ad
        /// </summary>
        public class Create {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
        }
    }
}