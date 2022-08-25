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
        Id = id;
        OwnerId = ownerId;
        State = ClassifiedAdState.Inactive;
        EnsureValidState();
        
        // Change in state captured as an event, an immutable fact
        Apply(new Events.ClassifiedAdCreated {
            Id = id,
            OwnerId = ownerId
        });
    }

    public void SetTitle(ClassifiedAdTitle title) {
        Title = title;
        EnsureValidState();
        
        Apply(new Events.ClassifiedAdTitleChanged {
            Id = Id,
            Title = title
        });
    }

    public void UpdateText(ClassifiedAdText text) {
        Text = text;
        EnsureValidState();
        
        Apply(new Events.ClassifiedAdTextUpdated {
            Id = Id,
            AdText = text
        });
    }

    public void UpdatePrice(Price price) {
        Price = price;
        EnsureValidState();
        
        Apply(new Events.ClassifiedAdPriceUpdated {
            Id = Id,
            Price = Price.Amount
        });
    }


    public void SubmitAdForPublishing() {
        State = ClassifiedAdState.PendingReview;
        EnsureValidState();
        
        Apply(new Events.ClassifiedAdSentForReview{Id = Id});
    }

    /// <summary>
    /// Apply the event to current aggregate state.
    /// </summary>
    /// <param name="event"></param>
    protected override void When(object @event) {
        switch (@event) {
            case Events.ClassifiedAdCreated e:
                Id = new ClassifiedAdId(e.Id);
                OwnerId = new UserId(e.OwnerId);
                State = ClassifiedAdState.Inactive;
                break;
            case Events.ClassifiedAdTitleChanged e:
                Title = new ClassifiedAdTitle(e.Title);
                break;
            case Events.ClassifiedAdTextUpdated e:
                Text = new ClassifiedAdText(e.AdText);
                break;
            case Events.ClassifiedAdPriceUpdated e:
                Price = new Price(e.Price, e.CurrencyCode);
                break;
            case Events.ClassifiedAdSentForReview e:
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
