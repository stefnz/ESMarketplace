using Marketplace;
using Marketplace.Api;
using Marketplace.Domain;

using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
    options.SwaggerDoc("v1", new OpenApiInfo {Title = "ClassifiedAds", Version = "v1"}));

builder.Services.AddSingleton<ICurrencyLookup, FixedCurrencyLookup>();
builder.Services.AddSingleton<ClassifiedAdUseCases>();
//builder.Services.AddSingleton<IAggregateStore>(RavenDbAggregateStore());
//builder.Services.AddScoped<ICommandHandler<ClassifiedAdContract.V1.Create>, CreateClassifiedAdHandler>();

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