using System;
using System.Linq.Expressions;
using DataAccessLayer.Entities;

namespace DataAccessLayer.RepositoryContract;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetProducts();
  

    Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression);

    Task<Product?> GetProductByCondition(Expression<Func<Product,bool>> conditionexpression);

    Task<Product> AddProduct(Product product);

    Task<Product?> UpdateProduct(Product product);

    Task<bool> DeleteProduct(Guid ProductID);
}
