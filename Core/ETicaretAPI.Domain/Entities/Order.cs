﻿using ETicaretAPI.Domain.Entities.Common;

namespace ETicaretAPI.Domain.Entities
{
    public class Order : BaseEntity
    {
        public string Description { get; set; }
        public string Address { get; set; }
        public Customer Customer { get; set; }
        public int CustomerId { get; set; }
        public ICollection<Product> Products { get; set; }
    }
}
