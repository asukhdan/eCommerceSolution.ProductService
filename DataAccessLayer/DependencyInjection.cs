using System;
using DataAccessLayer.Context;
using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoryContract;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DataAccessLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddDataLayerDependency(this IServiceCollection service,IConfiguration configuration)
    {
        service.AddDbContext<ApplicationDbContext>(options=>options.UseMySQL(configuration.GetConnectionString("DefaultConnection")!));
        service.AddScoped<IProductRepository,ProductRepository>();
        return service;

    }
}
