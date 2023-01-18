using ES.Framework;
using EventStore.ClientAPI;

using Marketplace.Infrastructure;
using Marketplace.ClassifiedAds;
using Marketplace.Projections;
using Marketplace.Projections.Upcasters;
using Marketplace.UserProfiles;

using Microsoft.OpenApi.Models;
using Raven.Client.Documents;
using Raven.Client.Documents.Session;
using Raven.Client.ServerWide;
using Raven.Client.ServerWide.Operations;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

// ReSharper disable UnusedMember.Global

namespace Marketplace {
    public class StartupWithEventStore {
        public StartupWithEventStore(IHostingEnvironment environment, IConfiguration configuration) {
            Environment = environment;
            Configuration = configuration;
        }

        private IConfiguration Configuration { get; }
        private IHostingEnvironment Environment { get; }

        public void ConfigureServices(IServiceCollection services) {
            var esConnection = EventStoreConnection.Create(
                Configuration["eventStore:connectionString"],
                ConnectionSettings.Create().KeepReconnecting(),
                Environment.ApplicationName);
            
            var store = new AggregateStore(esConnection);
            var profanityCheckingProxy = new ProfanityCheckingProxy();
            
            var documentStore = ConfigureRavenDb(Configuration.GetSection("ravenDb"));
            Func<IAsyncDocumentSession> getSession = () => documentStore.OpenAsyncSession();
            services.AddTransient(c => getSession());

            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);
            
            services.AddSingleton(new ClassifiedAdsUseCases(store, new FixedCurrencyLookup()));
            services.AddSingleton(new UserProfileUseCases(store, t => profanityCheckingProxy.CheckForProfanity(t)));

            var classifiedAdDetails = new List<ReadModels.ClassifiedAdDetails>(); // in-memory container for the query models
            services.AddSingleton<IEnumerable<ReadModels.ClassifiedAdDetails>>(classifiedAdDetails);

            var userDetails = new List<ReadModels.UserDetails>();
            services.AddSingleton<IEnumerable<ReadModels.UserDetails>>(userDetails);
            
            var projectionManager = new ProjectionsManager(
                connection: esConnection,
                checkpointStore: new RavenDbCheckpointStore(getSession, "readmodels"),
                projections: new IProjection[] { 
                    new ClassifiedAdDetailsProjection(
                        getSession,
                        async userId => (await getSession.GetUserDetails(userId))?.DisplayName),
                    
                    new UserDetailsProjection(getSession),
                    
                    new ClassifiedAdUpcasters(esConnection, 
                        async userId => (await getSession.GetUserDetails(userId))?.PhotoUrl)
                
                });
            
            services.AddSingleton<IHostedService>(new EventStoreHostedService(esConnection, projectionManager));
            
            services.AddMvc( options => options.EnableEndpointRouting = false);
            
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", 
                    new OpenApiInfo {
                        Title = "ClassifiedAds",
                        Version = "v1"
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env) {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifiedAds v1"));
        }
        
        private static IDocumentStore ConfigureRavenDb(IConfiguration configuration) {
            var store = new DocumentStore {
                Urls = new[] {configuration["server"]},
                Database = configuration["database"]
            };
            store.Initialize();
            var record = store.Maintenance.Server.Send(new GetDatabaseRecordOperation(store.Database));
            if (record == null) {
                store.Maintenance.Server.Send(new CreateDatabaseOperation(new DatabaseRecord(store.Database)));
            }

            return store;
        }
    }
}