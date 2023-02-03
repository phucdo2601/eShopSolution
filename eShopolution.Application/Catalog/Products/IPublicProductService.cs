
using eShopolution.Application.Catalog.Products.Dtos;
using eShopolution.Application.Dtos;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace eShopolution.Application.Catalog.Products
{
    public interface IPublicProductService
    {
        Task<PagedResult<ProductViewModel>> GetAllByCategoryId(Dtos.Public.GetProductPagingRequest request);
    }
}
