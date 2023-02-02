using eShopolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopolution.Application.Catalog.Products.Dtos.Manage
{
    public class GetProductPagingRequest : PagingRequestPage
    {
        public string Keyword { get; set; }

        public List<int> CategoryIds { get; set; }
    }
}
