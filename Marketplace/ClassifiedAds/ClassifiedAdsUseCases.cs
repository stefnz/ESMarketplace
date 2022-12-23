using ES.Framework;
using Marketplace.Contracts;
using Marketplace.Domain;

namespace Marketplace.ClassifiedAds;

public class ClassifiedAdsUseCases : IUseCases {
    private readonly ICurrencyLookup _currencyLookup;
    private readonly IAggregateStore _store;

    public ClassifiedAdsUseCases(IAggregateStore store, ICurrencyLookup currencyLookup) {
        _currencyLookup = currencyLookup;
        _store = store;
    }

    public Task Handle(object command) =>
        command switch {
            ClassifiedAdContract.V1.Create cmd =>
                HandleCreate(cmd),
            ClassifiedAdContract.V1.SetTitle cmd =>
                HandleUpdate(
                    cmd.Id,
                    c => c.SetTitle(
                        ClassifiedAdTitle
                            .FromString(cmd.Title)
                    )
                ),
            ClassifiedAdContract.V1.UpdateText cmd =>
                HandleUpdate(
                    cmd.Id,
                    c => c.UpdateText(
                        ClassifiedAdText
                            .FromString(cmd.Text)
                    )
                ),
            ClassifiedAdContract.V1.UpdatePrice cmd =>
                HandleUpdate(
                    cmd.Id,
                    c => c.UpdatePrice(
                        Price.FromDecimal(
                            cmd.Price,
                            cmd.Currency,
                            _currencyLookup
                        )
                    )
                ),
            ClassifiedAdContract.V1.RequestPublish cmd =>
                HandleUpdate(
                    cmd.Id,
                    c => c.SubmitAdForPublishing()
                ),
            ClassifiedAdContract.V1.Publish cmd =>
                HandleUpdate(
                    cmd.Id,
                    c => c.Publish(new UserId(cmd.ApprovedBy))
                ),
            _ => Task.CompletedTask
        };

    private async Task HandleCreate(ClassifiedAdContract.V1.Create cmd) {
        if (await _store.Exists<Domain.ClassifiedAd, ClassifiedAdId>(new ClassifiedAdId(cmd.Id))) {
            throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
        }

        var classifiedAd = new Domain.ClassifiedAd(new ClassifiedAdId(cmd.Id), new UserId(cmd.OwnerId));
        
        await _store.Save<Domain.ClassifiedAd, ClassifiedAdId>(classifiedAd);
    }

    private Task HandleUpdate(Guid id, Action<Domain.ClassifiedAd> update)
        => this.HandleUpdate(_store, new ClassifiedAdId(id), update);
}