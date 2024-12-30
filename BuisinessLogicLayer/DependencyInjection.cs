using System;
using BuisinessLogicLayer.Mappers;
using BuisinessLogicLayer.ServiceContract;
using BuisinessLogicLayer.Services;
using BuisinessLogicLayer.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace BuisinessLogicLayer;

public static class DependencyInjection
{
    public static IServiceCollection AddBusinessLayerDependency(this IServiceCollection service)
    {

        service.AddAutoMapper(typeof(ProductAddRequestToProductMappingProfile).Assembly);
        service.AddValidatorsFromAssemblyContaining<ProductAddRequestValidator>();
        service.AddScoped<IProductService,ProductService>();
        return service;

    }
}
