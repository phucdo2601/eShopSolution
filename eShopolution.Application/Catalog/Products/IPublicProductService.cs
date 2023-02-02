
using eShopolution.Application.Catalog.Products.Dtos;
using eShopolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace eShopolution.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        public PagedResult<ProductViewModel> GetAllByCategoryId(Dtos.Public.GetProductPagingRequest request);
    }
}
