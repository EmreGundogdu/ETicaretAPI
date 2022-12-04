﻿using ETicaretAPI.Application.Abstractions;
using ETicaretAPI.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Persistence.Concretes
{
    public class ProductService : IProductService
    {
        public List<Product> GetProducts() => new()
        {
            new() { ID=Guid.NewGuid(), Name = "Product 1", Price = 100, Stock = 10},
            new() { ID=Guid.NewGuid(), Name = "Product 2", Price = 200, Stock = 20},
            new() { ID=Guid.NewGuid(), Name = "Product 3", Price = 300, Stock = 30},
            new() { ID=Guid.NewGuid(), Name = "Product 4", Price = 400, Stock = 40},
            new() { ID=Guid.NewGuid(), Name = "Product 5", Price = 500, Stock = 50},
        };
    }
}
