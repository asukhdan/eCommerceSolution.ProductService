using DataAccessLayer;
using BuisinessLogicLayer;
using FluentValidation.AspNetCore;
using ProductMicroservice.API.Middleware;
using ProductMicroservice.API.APIEndPoints;
using System.Text.Json.Serialization;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        //Add BAL And DLL dependency
        builder.Services.AddDataLayerDependency(builder.Configuration);
        builder.Services.AddBusinessLayerDependency();

        builder.Services.AddControllers();

        //Add Fluent Validation 
        builder.Services.AddFluentValidationAutoValidation();
    


        //Add model binder to read values from JSON to enum
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        //Adding Swagger
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();



        var app = builder.Build();
        app.UseExceptionHandlingMiddleware();
        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();
        
        app.MapControllers();

        //Product Endpoint API
        app.MapProductAPIEndPoints();
        
        //Enable Swagger

        app.UseSwagger();
        app.UseSwaggerUI();;
        app.UseHttpsRedirection();
        // app.MapGet("/", () => "Hello World!");

        app.Run();
    }
}