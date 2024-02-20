using Ad.Application;
using Ad.Application.JsonOptions;
using Ad.Infrastructure;
using Ad.Infrastructure.Context;
using DbMigrator;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Minio;



var builder = WebApplication.CreateBuilder(args);

#region подключаю DbContext для ADService

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AdContext>(options => options.UseNpgsql(connectionString));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

#endregion

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddHttpClient();

builder.Services.AddSwaggerGen();

#region Подключаю Minio
builder.Services.Configure<MinioOptions>(builder.Configuration.GetSection(MinioOptions.Name));
var minioOptions = builder.Configuration.GetSection(MinioOptions.Name).Get<MinioOptions>()!;

// Add Minio using the custom endpoint and configure additional settings for default MinioClient initialization
builder.Services.AddMinio(configureClient => configureClient
    .WithEndpoint(minioOptions.Endpoint)
    .WithCredentials(minioOptions.AccessKey, minioOptions.SecretKey));
builder.Services.AddMinio(minioOptions.AccessKey, minioOptions.SecretKey);

// NOTE: SSL and Build are called by the build-in services already.
#endregion

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddApplicationService();
builder.Services.AddInfrastructureServices();
builder.Services.AddControllers();

#region CORS политики

builder.Services.AddCors(options =>
{
    options.AddPolicy("mypolicy", builder =>
    {
        builder.AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

#endregion


var app = builder.Build();

_ = app.Services.ApplyMigrations<AdContext>();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapControllers();

app.MapControllers();
app.UseCors("mypolicy");


app.UseRouting();
app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
app.Run();

