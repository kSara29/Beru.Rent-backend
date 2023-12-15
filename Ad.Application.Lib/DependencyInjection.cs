﻿using Ad.Application.Lib.Contracts.Tarif;
using Ad.Application.Lib.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Ad.Application.Lib;

public static class DependencyInjection
{
    public static IServiceCollection AddApplicationService(this IServiceCollection services)
    {
        services.AddScoped<ITarifService, TarifService>();
        return services;
    }
}