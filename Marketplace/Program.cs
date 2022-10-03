using ES.Framework;
using Marketplace;
using Marketplace.Api;
using Marketplace.Domain;
using Marketplace.Domain.UserProfiles;
using Marketplace.Infrastructure;
using Marketplace.UserProfiles;
using Microsoft.OpenApi.Models;
using Raven.Client.Documents;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "ClassifiedAds", Version = "v1"}));

// Configure RavenDb
var store = new DocumentStore() {
    Urls = new []{"http://localhost:8080"},
    Database = "Marketplace",
    Conventions = {
        FindIdentityProperty = memberInfo => memberInfo.Name == "DbId"
    }
};

store.Initialize();

var checkForProfanityClient = new ProfanityCheckingProxy();
builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddScoped(_ => store.OpenAsyncSession());
builder.Services.AddScoped<IUnitOfWork, RavenDbUnitOfWork>();
builder.Services.AddScoped<IClassifiedAdRepository, ClassifiedAdRepository>();
builder.Services.AddScoped<ClassifiedAdUseCases>();

builder.Services.AddScoped<IUserProfileRepository, UserProfileRepository>();
builder.Services.AddScoped(c => new UserProfileUseCases(
    c.GetService<IUserProfileRepository>(),
    c.GetService<IUnitOfWork>(),
    text => checkForProfanityClient.CheckForProfanity(text).GetAwaiter().GetResult()
    ));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();