using System;
using BuisinessLogicLayer.DTO;
using FluentValidation;

namespace BuisinessLogicLayer.Validators;

public class ProductUpdateRequestValidator:AbstractValidator<ProductUpdateRequest>
{
     public ProductUpdateRequestValidator()
    {
       RuleFor(temp => temp.ProductName).NotEmpty().WithMessage("Product Name Can't be blank");
       RuleFor(temp=>temp.Category).NotEmpty().WithMessage("Product Category Can't be blank");
       RuleFor(temp=>temp.UnitPrice).InclusiveBetween(0,double.MaxValue).WithMessage($"Unit price should be between 0 to {double.MaxValue}");
       RuleFor(temp=>temp.QuantityInStock).InclusiveBetween(0,int.MaxValue).WithMessage($"Quantity In Stock should be between 0 to {int.MaxValue}");

    }

}
