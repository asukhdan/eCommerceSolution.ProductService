using System;

namespace BuisinessLogicLayer.DTO;

public record ProductUpdateRequest(Guid ProductID ,string? ProductName,string? Category,double UnitPrice,int QuantityInStock)
{
    public ProductUpdateRequest(): this(default,default,default,default,default)
    {
    }
}
