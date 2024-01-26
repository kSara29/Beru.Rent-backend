using Deal.Infrastructure.Persistance;
using Microsoft.EntityFrameworkCore;
using Deal.Application;
using Deal.Infrastructure;
using FastEndpoints;
using Minio;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DealContext>(options =>
    options.UseNpgsql(connectionString), ServiceLifetime.Scoped);
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#region Подключаю Minio
var endpoint = "play.min.io";
var accessKey = "Q3AM3UQ867SPQQA43P2F";
var secretKey = "zuf+tfteSlswRu7BJ86wtrueekitnifILbZam1KYY3TG";

builder.Services.AddMinio(accessKey, secretKey);
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(endpoint)
    .WithCredentials(accessKey, secretKey));
#endregion

builder.Services.AddHttpClient();
builder.Services.AddFastEndpoints();
// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddInfrastructureServices();
builder.Services.AddApplicationService();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();
app.Run();
