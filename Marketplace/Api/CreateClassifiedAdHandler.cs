using ES.Framework;
using Marketplace.Contracts;
using Marketplace.Domain;

namespace Marketplace.Api;

public class CreateClassifiedAdHandler : ICommandHandler<ClassifiedAdContract.V1.Create> {
    private readonly IAggregateStore _store;
    public CreateClassifiedAdHandler(IAggregateStore store) => _store = store;

    public Task Handle(ClassifiedAdContract.V1.Create command) {
        //if (await _store.Exists(command.Id.ToString())) {
        //    throw new InvalidOperationException($"Entity with id {command.Id} already exists");
        //}

        var classifiedAd = new ClassifiedAd(new ClassifiedAdId(command.Id), new UserId(command.OwnerId));
        return _store.Save(classifiedAd);
    }
}