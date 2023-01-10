using ES.Framework;
using EventStore.ClientAPI;

using Marketplace.Infrastructure;
using Marketplace.ClassifiedAds;
using Marketplace.UserProfiles;

using Microsoft.OpenApi.Models;
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

            services.AddSingleton(esConnection);
            services.AddSingleton<IAggregateStore>(store);
            
            services.AddSingleton(new ClassifiedAdsUseCases(store, new FixedCurrencyLookup()));
            services.AddSingleton(new UserProfileUseCases(store, t => profanityCheckingProxy.CheckForProfanity(t)));

            var items = new List<ReadModels.ClassifiedAdDetails>();
            services.AddSingleton<IEnumerable<ReadModels.ClassifiedAdDetails>>(items);
            var subscription = new ClassifiedAdEventsSubscription(esConnection, items);
            services.AddSingleton<IHostedService>(new EventStoreHostedService(esConnection, subscription));
            
            services.AddMvc( options => options.EnableEndpointRouting = false);
            
            services.AddSwaggerGen(options => {
                options.SwaggerDoc("v1", 
                    new OpenApiInfo()
                    {
                        Title = "ClassifiedAds",
                        Version = "v1"
                    });
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvcWithDefaultRoute();
            app.UseSwagger();
            app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "ClassifiedAds v1"));
        }
    }
}