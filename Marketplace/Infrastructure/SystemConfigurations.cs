using System.Reflection;
using ES.Framework;
using EventStore.ClientAPI;
using Marketplace.Api;
using Raven.Client.Documents;

namespace Marketplace.Infrastructure;

public static class SystemConfigurations {
    public void ConfigureRavenDbStore(WebApplicationBuilder builder) {
// Configure RavenDb
        var store = new DocumentStore() {
            Urls = new []{"http://localhost:8080"},
            Database = "Marketplace",
            Conventions = {
                FindIdentityProperty = memberInfo => memberInfo.Name == "DbId"
            }
        };

        store.Initialize();        
    }


    public void ConfigureEventStore(WebApplicationBuilder builder) {
        var configuration = builder.Configuration
            .SetBasePath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location))
            .AddJsonFile("appsettings.json", false, false)
            .Build();

        var esConnection = EventStoreConnection.Create(
            configuration["eventStore:connectionString"],
            ConnectionSettings.Create().KeepReconnecting(),
            "ESMarketPlace");

        var store = new AggregateStore(esConnection);
        var profanityCheckProxy = new ProfanityCheckingProxy();
        
        builder.Services.AddSingleton(esConnection);
        builder.Services.AddSingleton<IAggregateStore>(store);
        
        builder.Services.AddSingleton(new ClassifiedAdUseCases(store, ))
    }
    

}