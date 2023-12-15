﻿using Ad.Application.Lib.Services;
using Ad.Infrastructure.Lib.Database;
using Microsoft.Extensions.DependencyInjection;

namespace Ad.Infrastructure.Lib;

public static class DependencyInjection
{ 
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services)
        {
                services.AddScoped<IPictureRepository, PictureRepository>();
                services.AddScoped<PictureDbContext>();
                return services;
        }
    
}