using eShopolution.Application.Catalog.Products.Dtos;
using eShopolution.Application.Catalog.Products.Dtos.Manage;
using eShopolution.Application.Dtos;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eShopolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _eShopDbContext;

        public ManageProductService(EShopDbContext eShopDbContext)
        {
            _eShopDbContext= eShopDbContext;
        }

        public async Task AddViewCount(int productId)
        {
            var product = await _eShopDbContext.Products.FindAsync(productId);
            product.ViewCount += 1;

            await _eShopDbContext.SaveChangesAsync();
        }

        public async Task<int> Create(ProductCreateRequest request)
        {
            var product = new Product()
            {
                Price = request.Price,
                OriginalPrice = request.OriginalPrice,
                Stock = request.Stock,
                ViewCount = 0,
                DateCreated = DateTime.Now,
                ProductTranslations = new List<ProductTranslation>()
                {
                    new ProductTranslation()
                    {
                        Name= request.Name,
                        Description = request.Description,
                        Details = request.Details,
                        SeoDescription = request.SeoDescription,
                        SeoAlias= request.SeoAlias,
                        SeoTitle= request.SeoTitle,
                        LanguageId =request.LanguageId,
                    }
                }
            };

            _eShopDbContext.Products.Add(product);

            return await _eShopDbContext.SaveChangesAsync();
        }

        public async Task<int> Delete(int productId)
        {
            var product = await _eShopDbContext.Products.FindAsync(productId);

            if (product == null)
            {
                throw new EShopException($"Cannot find a product: {productId}");
            }

            _eShopDbContext.Products.Remove(product);

            return await _eShopDbContext.SaveChangesAsync();
        }

        public Task<List<ProductViewModel>> GetAll()
        {
            throw new NotImplementedException();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            //1. Select Join
            var query = from p in _eShopDbContext.Products
                        join pt in _eShopDbContext.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _eShopDbContext.ProductInCategories on p.Id equals pic.ProductId
                        join c in _eShopDbContext.Categories on pic.CategoryId equals c.Id
                        where pt.Name.Contains(request.Keyword)
                        select new {p, pt, pic};

            //2. Filter 
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query =  query.Where(x => x.pt.Name.Contains(request.Keyword));

            }

            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));

            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex -1) * request.PageSize).Take(request.PageSize)
                 .Select(x => new ProductViewModel()
                 {
                     Id = x.p.Id,
                     Name = x.pt.Name,
                     DateCreated = x.p.DateCreated,
                     Description = x.pt.Description,
                     Details = x.pt.Details,
                     LanguageId = x.pt.LanguageId,
                     OriginalPrice = x.p.OriginalPrice,
                     Price = x.p.Price,
                     SeoAlias = x.pt.SeoAlias,
                     SeoDescription = x.pt.SeoDescription,
                     SeoTitle = x.pt.SeoTitle,
                     Stock = x.p.Stock,
                     ViewCount = x.p.ViewCount
                 }).ToListAsync();

            //4. Select and projection
            var pagedResult = new PagedResult<ProductViewModel>()
            {
                TotalRecord = totalRow,
                Items = data,
            };
            return pagedResult;
        }

        public Task<int> Update(ProductUpdateRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<int> UpdatePrice(int productId, decimal newPrice)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            throw new NotImplementedException();
        }
    }
}
