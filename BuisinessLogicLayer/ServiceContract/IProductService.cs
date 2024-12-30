using System;
using System.Linq.Expressions;
using BuisinessLogicLayer.DTO;
using DataAccessLayer.Entities;

namespace BuisinessLogicLayer.ServiceContract;

public interface IProductService
{
    Task<List<ProductResponse?>> GetProducts();

    Task<List<ProductResponse?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionexpression);

    Task<ProductResponse?> GetProductByCondition(Expression<Func<Product, bool>> conditionexpression);

    Task<ProductResponse?> AddProduct(ProductAddRequest productaddRequest);

    Task<ProductResponse?> UpdateProduct(ProductUpdateRequest productUpdateRequest);

    Task<bool> DeleteProduct(Guid ProductID);




}
