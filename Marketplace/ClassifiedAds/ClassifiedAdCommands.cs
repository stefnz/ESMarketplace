// Contract or Command?
// This contract contains the commands available to interact with the domain.
//
// Given this follows Command Query Responsibility Separation (CQRS), should this use the term Commands.

namespace Marketplace.Contracts;

public static class ClassifiedAdContract {
    /// <summary>
    /// DTOs for the Classified Ad resource
    /// </summary>
    public static class V1 {
        /// <summary>
        /// Command to create a new classified ad
        /// </summary>
        public class Create {
            public Guid Id { get; set; }
            public Guid OwnerId { get; set; }
        }

        public class SetTitle {
            public Guid Id { get; set; }
            public string Title { get; set; }
        }

        public class UpdateText {
            public Guid Id { get; set; }
            public string Text { get; set; }
        }

        public class UpdatePrice {
            public Guid Id { get; set; }
            public decimal Price { get; set; }
            public string Currency { get; set; }
        }

        public class RequestPublish {
            public Guid Id { get; set; }
            public Guid ApprovedBy { get; set; }
        }

        public class Publish {
            public Guid Id { get; set; }
            public Guid ApprovedBy { get; set; }
        }
    }
}