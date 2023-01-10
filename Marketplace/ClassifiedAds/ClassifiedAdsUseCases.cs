using ES.Framework;
using Marketplace.Contracts;
using Marketplace.Domain;

namespace Marketplace.ClassifiedAds;

public class ClassifiedAdsUseCases : IUseCases {
    private readonly ICurrencyLookup currencyLookup;
    private readonly IAggregateStore store;

    public ClassifiedAdsUseCases(IAggregateStore store, ICurrencyLookup currencyLookup) {
        this.currencyLookup = currencyLookup;
        this.store = store;
    }

    public Task Handle(object command) =>
        command switch {
            ClassifiedAdContract.V1.Create cmd =>
                HandleCreate(cmd),
            ClassifiedAdContract.V1.SetTitle cmd =>
                HandleUpdate(
                    cmd.Id,
                    ad => ad.SetTitle(ClassifiedAdTitle.FromString(cmd.Title))
                ),
            ClassifiedAdContract.V1.UpdateText cmd =>
                HandleUpdate(
                    cmd.Id,
                    ad => ad.UpdateText(ClassifiedAdText.FromString(cmd.Text))
                ),
            ClassifiedAdContract.V1.UpdatePrice cmd =>
                HandleUpdate(
                    cmd.Id,
                    ad => ad.UpdatePrice(
                        Price.FromDecimal(
                            cmd.Price,
                            cmd.Currency,
                            currencyLookup
                        )
                    )
                ),
            ClassifiedAdContract.V1.RequestPublish cmd =>
                HandleUpdate(
                    cmd.Id,
                    ad => ad.SubmitAdForPublishing()
                ),
            ClassifiedAdContract.V1.Publish cmd =>
                HandleUpdate(
                    cmd.Id,
                    ad => ad.Publish(new UserId(cmd.ApprovedBy))
                ),
            _ => Task.CompletedTask
        };

    private async Task HandleCreate(ClassifiedAdContract.V1.Create cmd) {
        if (await store.Exists<Domain.ClassifiedAd, ClassifiedAdId>(new ClassifiedAdId(cmd.Id))) {
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
        }

        var classifiedAd = new Domain.ClassifiedAd(new ClassifiedAdId(cmd.Id), new UserId(cmd.OwnerId));
        
        await store.Save<Domain.ClassifiedAd, ClassifiedAdId>(classifiedAd);
    }

    private Task HandleUpdate(Guid id, Action<Domain.ClassifiedAd> update)
        => this.HandleUpdate(store, new ClassifiedAdId(id), update);
}