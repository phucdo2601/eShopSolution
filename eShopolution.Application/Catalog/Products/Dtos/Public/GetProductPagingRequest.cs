using eShopolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopolution.Application.Catalog.Products.Dtos.Public
{
    public class GetProductPagingRequest : PagingRequestPage
    {
        public int CategoyId { get; set; }
    }
}
