using System;

using System.Linq.Expressions;
using AutoMapper;
using BuisinessLogicLayer.DTO;
using BuisinessLogicLayer.ServiceContract;
using BuisinessLogicLayer.Validators;
using DataAccessLayer.Entities;
using DataAccessLayer.Repositories;
using DataAccessLayer.RepositoryContract;
using FluentValidation;
using FluentValidation.Results;

namespace BuisinessLogicLayer.Services;

public class ProductService : IProductService
{
    public readonly IMapper _mapper;
    public readonly IProductRepository _productRepository;
    public readonly IValidator<ProductAddRequest> _productaddrequestValidator;
    public readonly IValidator<ProductUpdateRequest> _productupdaterequestValidator;


    public ProductService(IProductRepository productRepository, IMapper mapper, IValidator<ProductAddRequest> productaddrequestvalidator
    , IValidator<ProductUpdateRequest> productupdaterequestvalidator)
    {
        _mapper = mapper;
        _productRepository = productRepository;
        _productaddrequestValidator = productaddrequestvalidator;
        _productupdaterequestValidator = productupdaterequestvalidator;
    }

    public async Task<ProductResponse?> AddProduct(ProductAddRequest productaddRequest)
    {
        if (productaddRequest == null)
        {
            throw new ArgumentNullException(nameof(productaddRequest));
        }
        //validate the product using Fluent Validation
        ValidationResult validationResult = await _productaddrequestValidator.ValidateAsync(productaddRequest);

        //check the validation results
        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        //Attempt to add product 
        Product productInput = _mapper.Map<Product>(productaddRequest);
        Product? addedProduct = await _productRepository.AddProduct(productInput);
        if (addedProduct == null)
        {
            return null;
        }
        ProductResponse addedproductResponse = _mapper.Map<ProductResponse>(addedProduct);
        return addedproductResponse;
    }

    public async Task<bool> DeleteProduct(Guid ProductID)
    {
        Product? existingProduct = await _productRepository.GetProductByCondition(temp => temp.ProductID == ProductID);
        if (existingProduct == null)
        {
            return false;
        }
        bool isDeleted = await _productRepository.DeleteProduct(ProductID);
        return isDeleted;
    }

    public async Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionexpression)
    {
        Product? product = await _productRepository.GetProductByCondition(conditionexpression);
        if (product == null)
        {
            return null;
        }
        ProductResponse productResponse = _mapper.Map<ProductResponse>(product);
        return productResponse;

    }

    public async Task<List<ProductResponse?>> GetProducts()
    {
        IEnumerable<Product?> products = await _productRepository.GetProducts();
        if (products == null)
        {
            return null;
        }
        IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
        return productResponses.ToList();
    }

    public async Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionexpression)
    {
        IEnumerable<Product?> products = await _productRepository.GetProductsByCondition(conditionexpression);

        IEnumerable<ProductResponse?> productResponses = _mapper.Map<IEnumerable<ProductResponse>>(products);
        return productResponses.ToList();
    }

    public async Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest)
    {
        Product? existingProduct = await _productRepository.GetProductByCondition(temp => temp.ProductID == productUpdateRequest.ProductID);

        if (existingProduct == null)
        {
            throw new ArgumentException("Invalid Product Id");
        }

        ValidationResult validationResult = await _productupdaterequestValidator.ValidateAsync(productUpdateRequest);

        if (!validationResult.IsValid)
        {
            string errors = string.Join(", ", validationResult.Errors.Select(temp => temp.ErrorMessage));
            throw new ArgumentException(errors);
        }

        Product productTobeUpdate = _mapper.Map<Product>(productUpdateRequest);
        Product? updatedProduct = await _productRepository.UpdateProduct(productTobeUpdate);
        if (updatedProduct == null)
        {
            return null;
        }
        ProductResponse updateproductResponse = _mapper.Map<ProductResponse>(updatedProduct);
        return updateproductResponse;


    }
}
