using Ad.Application.Lib.Contracts.Ad;
using Ad.Application.Lib.Contracts.Tarif;
using Ad.Application.Lib.Contracts.Tag;
using Ad.Application.Lib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ad.Application.Lib;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<ITarifService, TarifService>();
        services.AddSingleton<ITagService, TagService>();
        services.AddScoped<IPictureService, PictureService>();
        services.AddSingleton<IAdService, AdService>();
        return services;
    }
}