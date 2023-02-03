using eShopSolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopSolution.ViewModel.Catalog.Products.Public
{
    public class GetProductPagingRequest : PagingRequestPage
    {
        public int? CategoryId { get; set; }
    }
}
