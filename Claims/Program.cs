using AutoMapper;
using Claims;
using Claims.Application.Services;
using Claims.Core.Repositories;
using Claims.Core.Services;
using Claims.Infrastructure;
using Claims.Infrastructure.Repositories;
using Claims.Web.Models;
using Claims.Web.Validators;
using FluentValidation;
using Microsoft.Azure.Cosmos;
using Microsoft.EntityFrameworkCore;
using SharpGrip.FluentValidation.AutoValidation.Mvc.Extensions;
using System.Text.Json.Serialization;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers().AddJsonOptions(x =>
    {
        x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    }
);

builder.Services.AddSingleton(await
    InitializeCosmosClientInstanceAsync(builder.Configuration.GetSection("CosmosDb")));

builder.Services.AddDbContext<AuditContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

builder.Services.Configure<CosmosDbOptions>(builder.Configuration.GetSection("CosmosDb"));

var mapperConfig = new MapperConfiguration(mc =>
{
    mc.AddProfile(new MappingProfile());
});

IMapper mapper = mapperConfig.CreateMapper();
builder.Services.AddSingleton(mapper);
builder.Services.AddScoped<IAuditRepository, AuditRepository>();
builder.Services.AddScoped<ICoverRepository, CoverRepository>();
builder.Services.AddScoped<IClaimRepository, ClaimRepository>();
builder.Services.AddScoped<IClaimService, ClaimService>();
builder.Services.AddScoped<ICoverService, CoverService>();
builder.Services.AddScoped<IPremiumCalculatorService, PremiumCalculatorService>();
builder.Services.AddScoped<IValidator<Claim>, ClaimValidator>();

builder.Services.AddFluentValidationAutoValidation();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<AuditContext>();
    context.Database.Migrate();
}

app.Run();

static async Task<CosmosClient> InitializeCosmosClientInstanceAsync(IConfigurationSection configurationSection)
{
    string account = configurationSection.GetSection("Account").Value ?? throw new ArgumentException("Account");
    string key = configurationSection.GetSection("Key").Value ?? throw new ArgumentException("Key");

    CosmosClient client = new CosmosClient(account, key);

    string databaseName = configurationSection.GetSection("DatabaseName").Value ?? throw new ArgumentException("DatabaseName");
    string coverContainerName = configurationSection.GetSection("CoverContainerName").Value ?? throw new ArgumentException("CoverContainerName");
    string claimContainerName = configurationSection.GetSection("ClaimContainerName").Value ?? throw new ArgumentException("ClaimContainerName");

    DatabaseResponse database = await client.CreateDatabaseIfNotExistsAsync(databaseName);

    await database.Database.CreateContainerIfNotExistsAsync(coverContainerName, "/id");
    await database.Database.CreateContainerIfNotExistsAsync(claimContainerName, "/id");

    return client;
}

public partial class Program { }