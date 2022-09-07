using ES.Framework;

namespace Marketplace.Domain;

public class ClassifiedAd: Aggregate {
    public ClassifiedAdId Id { get; private set; }
    public UserId OwnerId { get; private set; }
    public ClassifiedAdTitle Title { get; private set; }
    public ClassifiedAdText Text { get; private set; }
    public Price Price { get; private set; }
    public ClassifiedAdState State { get; private set; }
    public UserId ApprovedBy { get; private set; }


    public ClassifiedAd(ClassifiedAdId id, UserId ownerId) {
        // Change in state captured as an event, an immutable fact
        Apply(new ClassifiedAdEvents.ClassifiedAdCreated {
            Id = id,
            OwnerId = ownerId
        });
    }

    public void SetTitle(ClassifiedAdTitle title) {
        Apply(new ClassifiedAdEvents.ClassifiedAdTitleChanged {
            Id = Id,
            Title = title
        });
    }

    public void UpdateText(ClassifiedAdText text) {
        Apply(new ClassifiedAdEvents.ClassifiedAdTextUpdated {
            Id = Id,
            AdText = text
        });
    }

    public void UpdatePrice(Price price) {
        Apply(new ClassifiedAdEvents.ClassifiedAdPriceUpdated {
            Id = Id,
            Price = price.Amount,
            CurrencyCode = price.Currency.CurrencyCode
        });
    }

    public void SubmitAdForPublishing() {
        Apply(new ClassifiedAdEvents.ClassifiedAdSentForReview{Id = Id});
    }

    /// <summary>
    /// Apply the event to current aggregate state.
    /// </summary>
    /// <param name="event"></param>
    protected override void When(object @event) {
        switch (@event) {
            case ClassifiedAdEvents.ClassifiedAdCreated e:
                Id = new ClassifiedAdId(e.Id);
                OwnerId = new UserId(e.OwnerId);
                State = ClassifiedAdState.Inactive;
                break;
            case ClassifiedAdEvents.ClassifiedAdTitleChanged e:
                Title = new ClassifiedAdTitle(e.Title);
                break;
            case ClassifiedAdEvents.ClassifiedAdTextUpdated e:
                Text = new ClassifiedAdText(e.AdText);
                break;
            case ClassifiedAdEvents.ClassifiedAdPriceUpdated e:
                Price = new Price(e.Price, e.CurrencyCode);
                break;
            case ClassifiedAdEvents.ClassifiedAdSentForReview _:
                State = ClassifiedAdState.PendingReview;
                break;
        }
    }

    protected override void EnsureValidState() {
        bool isValid =
            Id != null
            && OwnerId != null
            && (State switch {
                ClassifiedAdState.PendingReview =>
                    Title != null
                    && Text != null
                    && Price?.Amount > 0,
                ClassifiedAdState.Active =>
                    Title != null
                    && Text != null
                    && Price?.Amount > 0
                    && ApprovedBy != null,
                _ => true
            });

        if (!isValid) {
            // TODO: improve error capture and reporting
            throw new InvalidEntityStateException(this, $"Classified Ad validation failed in state {State}");
        }
    }
    public enum ClassifiedAdState {
        PendingReview,
        Active,
        Inactive,
        MarkedAsSold
    }
}
