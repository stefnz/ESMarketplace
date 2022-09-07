using ES.Framework;
using Marketplace.Contracts;
using Marketplace.Domain;

namespace Marketplace.Api; 

/// <summary>
/// Classified Ad use cases accessed by the API. 
/// </summary>
public class ClassifiedAdUseCases: IUseCases {
    private readonly IClassifiedAdRepository repository;
    private readonly ICurrencyLookup currencyLookup;
    
    public ClassifiedAdUseCases(IClassifiedAdRepository repository, ICurrencyLookup currencyLookup) {
        this.repository = repository;
        this.currencyLookup = currencyLookup;
    }

    public async Task Handle(object command) {
        switch (command) {
            case ClassifiedAdContract.V1.Create cmd:
                await CreateClassifiedAd(cmd);
                break;
            case ClassifiedAdContract.V1.SetTitle cmd:
                await SetAdTitle(cmd);
                break;
            case ClassifiedAdContract.V1.UpdateText cmd:
                await UpdateText(cmd);
                break;
            case ClassifiedAdContract.V1.UpdatePrice cmd:
                await UpdatePrice(cmd);
                break;
            case ClassifiedAdContract.V1.RequestPublish cmd:
                await SubmitAdForPublishing(cmd);
                break;
            default:
                throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown");
        }
    }

    private async Task CreateClassifiedAd(ClassifiedAdContract.V1.Create command) {
        if (await repository.Exists(command.Id.ToString())) {
            throw new InvalidOperationException($"Classified Ad with id {command.Id} already exists");
        }

        var classifiedAd = new ClassifiedAd(new ClassifiedAdId(command.Id), new UserId(command.OwnerId));
        await repository.Save(classifiedAd);
    }

    private async Task SetAdTitle(ClassifiedAdContract.V1.SetTitle command) {
        ClassifiedAd classifiedAd = await repository.Load(command.Id.ToString());
        
        if (classifiedAd == null) {
            throw new InvalidOperationException($"Classified Ad with id {command.Id} cannot be found");
        }
        
        classifiedAd.SetTitle(ClassifiedAdTitle.FromString(command.Title));
        await repository.Save(classifiedAd);
    }

    private async Task UpdateText(ClassifiedAdContract.V1.UpdateText command) {
        ClassifiedAd classifiedAd = await repository.Load(command.Id.ToString());
        if (classifiedAd == null) {
            throw new InvalidOperationException($"Classified ad with id {command.Id} cannot be found");
        }

        classifiedAd.UpdateText(ClassifiedAdText.FromString(command.Text));
        await repository.Save(classifiedAd);
    }

    private async Task UpdatePrice(ClassifiedAdContract.V1.UpdatePrice command) {
        ClassifiedAd classifiedAd = await repository.Load(command.Id.ToString());
        if (classifiedAd == null) {
            throw new InvalidOperationException($"Classified Ad with id {command.Id} cannot be found");
        }

        classifiedAd.UpdatePrice(Price.FromDecimal(command.Price, command.Currency, currencyLookup));
        await repository.Save(classifiedAd);        
    }

    private async Task SubmitAdForPublishing(ClassifiedAdContract.V1.RequestPublish command) {
        ClassifiedAd classifiedAd = await repository.Load(command.Id.ToString());
        if (classifiedAd == null) {
            throw new InvalidOperationException($"Classified Ad with id {command.Id} cannot be found");
        }

        classifiedAd.SubmitAdForPublishing();
        await repository.Save(classifiedAd);
    }
    
    // TODO: possible to simplify command handlers?
}