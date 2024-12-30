using System;
using BuisinessLogicLayer.DTO;
using BuisinessLogicLayer.ServiceContract;
using DataAccessLayer.Repositories;
using FluentValidation;
using FluentValidation.Results;
using MySqlX.XDevAPI.Common;

namespace ProductMicroservice.API.APIEndPoints;

public static class ProductAPIEndPoints
{
    public static IEndpointRouteBuilder MapProductAPIEndPoints(this IEndpointRouteBuilder app)
    {

        //GET /api/Products   
        app.MapGet("/api/products", async (IProductService productService) =>
        {
            List<ProductResponse?> products = await productService.GetProducts();
            return Results.Ok(products);
        });


        //Get /api/products/search/productID/{ProductID}
        app.MapGet("api/products/search/product-id/{ProductID:guid}", async (IProductService productService, Guid ProductID) =>
        {
            ProductResponse? product = await productService
                                           .GetProductByCondition(item => item.ProductID == ProductID);
            return Results.Ok(product);


        });

        //Get /api/products/search/{SearchString}
        app.MapGet("api/products/search/{SearchString}", async (IProductService productService, string SearchString) =>
        {
            List<ProductResponse?> productsByProductName = await productService
                                            .GetProductsByCondition(item => item.ProductName != null &&
                                            item.ProductName.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            List<ProductResponse?> productsByCategory = await productService
                                              .GetProductsByCondition(item => item.Category != null &&
                                              item.Category.Contains(SearchString, StringComparison.OrdinalIgnoreCase));

            var products = productsByProductName.Union(productsByCategory);


            return Results.Ok(products);
        });

        //Post /api/products
        app.MapPost("/api/products",
        async (IProductService productService,
              IValidator<ProductAddRequest> productAddRequestValidator,
               ProductAddRequest productAddRequest) =>
               {
                   //Validate the product add request object with FluentValidation
                   ValidationResult validationResult = await productAddRequestValidator.ValidateAsync(productAddRequest);

                   //Check the validation Result
                   if (!validationResult.IsValid)
                   {
                       Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                                                           .ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                       return Results.ValidationProblem(errors);
                   }

                   //Add Product using productservice
                   var addedproductResponse = await productService.AddProduct(productAddRequest);


                   if (addedproductResponse != null)
                   {
                       return Results.Created($"api/products/search/product-id/{addedproductResponse.ProductID}", addedproductResponse);
                   }
                   else
                   {
                       return Results.Problem("Error in adding Product");
                   }
               }
           );

        //PUT:   /api/products
        app.MapPut("/api/products",
          async (IProductService productService,
          IValidator<ProductUpdateRequest> productUpdateRequestValidator,
           ProductUpdateRequest productupdateRequest) =>
           {
               //Validate the product add request object with FluentValidation
               ValidationResult validationResult = await productUpdateRequestValidator.ValidateAsync(productupdateRequest);

               //Check the validation Result
               if (!validationResult.IsValid)
               {
                   Dictionary<string, string[]> errors = validationResult.Errors.GroupBy(temp => temp.PropertyName)
                                                       .ToDictionary(grp => grp.Key, grp => grp.Select(err => err.ErrorMessage).ToArray());
                   return Results.ValidationProblem(errors);
               }

               //Update Product using productservice
               var updatedproductResponse = await productService.UpdateProduct(productupdateRequest);


               if (updatedproductResponse != null)
               {
                   return Results.Ok(updatedproductResponse);
               }
               else
               {
                   return Results.Problem("Error in adding Product");
               }
           }
       );


        //DELETE:   /api/products/xxxx
        app.MapDelete("/api/products/{ProductID:guid}",
          async (IProductService productService,
                  Guid ProductID) =>
           {
               bool isDeleted = await productService.DeleteProduct(ProductID);
               
               //Check the validation Result
               if (isDeleted)
               {
                   return Results.Ok(true);
               }
               else
               {
                   return Results.Problem("Error in deleting Product");
               }
           }
       );
        //Returning App Objet back to program.cs
        return app;



    }

}
