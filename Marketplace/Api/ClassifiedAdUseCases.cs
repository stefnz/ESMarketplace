using ES.Framework;
using Marketplace.Contracts;
using Marketplace.Domain;

namespace Marketplace.Api; 

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
                CreateClassifiedAd(cmd);
                break;
            case ClassifiedAdContract.V1.SetTitle cmd:
                SetAdTitle(cmd);
                break;
            
            
            default:
                throw new InvalidOperationException(
                    $"Command type {command.GetType().FullName} is unknown");
        }
    }

    private async void CreateClassifiedAd(ClassifiedAdContract.V1.Create command) {
        if (await repository.Exists(command.Id.ToString())) {
            throw new InvalidOperationException($"Classified Ad with id {command.Id} already exists");
        }

        var classifiedAd = new ClassifiedAd(new ClassifiedAdId(command.Id), new UserId(command.OwnerId));
        await repository.Save(classifiedAd);
    }

    private async void SetAdTitle(ClassifiedAdContract.V1.SetTitle command) {
        ClassifiedAd classifiedAd = await repository.Load(command.Id.ToString());
        
        if (classifiedAd == null) {
            throw new InvalidOperationException($"Classified Ad with id {command.Id} cannot be found");
        }
        
        classifiedAd.SetTitle(ClassifiedAdTitle.FromString(command.Title));
        await repository.Save(classifiedAd);
    }


}