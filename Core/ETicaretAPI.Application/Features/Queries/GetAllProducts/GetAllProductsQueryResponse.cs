﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Features.Queries.GetAllProducts
{
    public class GetAllProductsQueryResponse
    {
        public int TotalCount { get; set; }
        public object Products { get; set; }
    }
}
