using System;

namespace BuisinessLogicLayer.DTO;

public record ProductAddRequest(string? ProductName,string? Category,double UnitPrice,int QuantityInStock)
{
    public ProductAddRequest(): this(default,default,default,default)
    {
    }
}

