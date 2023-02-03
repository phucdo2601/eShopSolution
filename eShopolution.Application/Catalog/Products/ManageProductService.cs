using eShopSolution.ViewModel.Catalog.Products;
using eShopSolution.ViewModel.Catalog.Products.Manage;
using eShopSolution.ViewModel.Common;
using eShopSolution.Data.EF;
using eShopSolution.Data.Entities;
using eShopSolution.Utilities.Exceptions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.IO;
using eShopolution.Application.Common;

namespace eShopolution.Application.Catalog.Products
{
    public class ManageProductService : IManageProductService
    {
        private readonly EShopDbContext _eShopDbContext;
        private readonly IStorageService _storageService;

        public ManageProductService(EShopDbContext eShopDbContext, IStorageService storageService)
        {
            _eShopDbContext = eShopDbContext;
            _storageService = storageService;
        }

        public Task<int> AddImages(int productId, List<IFormFile> files)
        {
            throw new NotImplementedException();
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

            // Save Image
            if (request.ThumbnailImage != null)
            {
                product.ProductImages = new List<ProductImage>()
                {
                    new ProductImage()
                    {
                        Caption = "Thumbnail Image",
                        DateCreated = DateTime.Now,
                        FileSize = request.ThumbnailImage.Length,
                        ImagePath = await this.SaveFile(request.ThumbnailImage),
                        IsDefault= true,
                        SortOrder = 1
                    }
                };
            }


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

            /**
             * Trong truong hop co file ton tai, thi phai xoa file vat ly truoc, sau do moi xoa du lieu
             */
            //delete file vat ly
            var images =  _eShopDbContext.ProductImages.Where(i => i.ProductId == productId);
            foreach (var image in images)
            {
                await _storageService.DeleteFileAsync(image.ImagePath);
            }

            _eShopDbContext.Products.Remove(product);

            


            return await _eShopDbContext.SaveChangesAsync();
        }

        public async Task<PagedResult<ProductViewModel>> GetAllPaging(GetProductPagingRequest request)
        {
            //1. Select Join
            var query = from p in _eShopDbContext.Products
                        join pt in _eShopDbContext.ProductTranslations on p.Id equals pt.ProductId
                        join pic in _eShopDbContext.ProductInCategories on p.Id equals pic.ProductId
                        join c in _eShopDbContext.Categories on pic.CategoryId equals c.Id
                        where pt.Name.Contains(request.Keyword)
                        select new { p, pt, pic };

            //2. Filter 
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(x => x.pt.Name.Contains(request.Keyword));

            }

            if (request.CategoryIds.Count > 0)
            {
                query = query.Where(p => request.CategoryIds.Contains(p.pic.CategoryId));

            }

            //3. Paging
            int totalRow = await query.CountAsync();

            var data = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize)
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

        public Task<List<eShopSolution.ViewModel.Catalog.Products.Public.ProductImageViewModel>> GetListImage(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<int> RemoveImages(int imageId)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Update(ProductUpdateRequest request)
        {
            var product = await _eShopDbContext.Products.FindAsync(request.Id);

            var productTranlations = await _eShopDbContext.ProductTranslations.FirstOrDefaultAsync(x => x.ProductId == request.Id && x.LanguageId == request.LanguageId);

            if (product == null || productTranlations == null)
            {
                throw new EShopException($"Cannot find a product with id: {request.Id}");
            }

            productTranlations.Name = request.Name;
            productTranlations.SeoAlias = request.SeoAlias;
            productTranlations.SeoDescription = request.SeoDescription;
            productTranlations.SeoTitle = request.SeoTitle;
            productTranlations.Description = request.Description;
            productTranlations.Details = request.Details;

            //Save image
            if (request.ThumbnailImage != null)
            {
                var thumbnailImage = await _eShopDbContext.ProductImages.FirstOrDefaultAsync(i => i.IsDefault == true && i.ProductId == request.Id);
                if (thumbnailImage != null)
                {
                    thumbnailImage.FileSize = request.ThumbnailImage.Length;
                    thumbnailImage.ImagePath = await this.SaveFile(request.ThumbnailImage);
                    _eShopDbContext.ProductImages.Update(thumbnailImage);
                }
            }

            return await _eShopDbContext.SaveChangesAsync();

        }

        public Task<int> UpdateImages(int imageId, string caption, bool isDefault)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> UpdatePrice(int productId, decimal newPrice)
        {
            var product = await _eShopDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EShopException($"Cannot find a product with id: {productId}");
            }

            product.Price = newPrice;
            return await _eShopDbContext.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateStock(int productId, int addedQuantity)
        {
            var product = await _eShopDbContext.Products.FindAsync(productId);
            if (product == null)
            {
                throw new EShopException($"Cannot find a product with id: {productId}");
            }

            product.Stock += addedQuantity;

            return await _eShopDbContext.SaveChangesAsync() > 0;
        }

        private async Task<string> SaveFile(IFormFile file)
        {
            var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
            var fileName = $"{Guid.NewGuid()}.{Path.GetExtension(originalFileName)}";
            await _storageService.SaveFileAsync(file.OpenReadStream(), fileName);
            return fileName;
        }
    }
}
