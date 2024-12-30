using System;
using System.Data.Common;
using System.Linq.Expressions;
using DataAccessLayer.Context;
using DataAccessLayer.Entities;
using DataAccessLayer.RepositoryContract;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Repositories;

public class ProductRepository : IProductRepository
{
    public readonly ApplicationDbContext _dbContext;
    public ProductRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;

    }

    public async Task<Product> AddProduct(Product product)
    {

        _dbContext.Products.Add(product);
        await _dbContext.SaveChangesAsync();
        return product;
    }

    public async Task<bool> DeleteProduct(Guid ProductID)
    {
        Product? existingProduct = await _dbContext.Products.FirstOrDefaultAsync(temp => temp.ProductID == ProductID);
        if (existingProduct == null)
        {
            return false;
        }
        _dbContext.Products.Remove(existingProduct);
        int affectdRowsCount = await _dbContext.SaveChangesAsync();

        return affectdRowsCount > 0;
    }

    public async Task<Product?> GetProductByCondition(Expression<Func<Product, bool>> conditionexpression)
    {
        return await _dbContext.Products.FirstOrDefaultAsync(conditionexpression);
    }

    public async Task<IEnumerable<Product>> GetProducts()
    {
       return await _dbContext.Products.ToListAsync();
    }

    public async Task<IEnumerable<Product?>> GetProductsByCondition(Expression<Func<Product, bool>> conditionExpression)
    {
        return await _dbContext.Products.Where(conditionExpression).ToListAsync();
    }

    public async Task<Product?> UpdateProduct(Product product)
    {
        Product? existingProduct = await _dbContext.Products
        .FirstOrDefaultAsync(temp => temp.ProductID == product.ProductID);
        if (existingProduct == null)
        {
            return null;
        }
        existingProduct.ProductName = product.ProductName;
        existingProduct.Category = product.Category;
        existingProduct.QuantityInStock = product.QuantityInStock;
        existingProduct.UnitPrice = product.UnitPrice;

        await _dbContext.SaveChangesAsync();
        return existingProduct;

    }
}
