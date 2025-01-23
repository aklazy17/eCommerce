using eCommerce.ProductDetail.API.Data;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.ProductDetail.API.Repositories;

public class ProductDetailRepository : IProductDetailRepository
{
    private readonly AppDbContext _db;

    public ProductDetailRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Models.ProductDetail>> AddAsync(List<Models.ProductDetail> productDetails)
    {
        var productIds = productDetails.Select(x => x.ProductId).Distinct().ToList();

        foreach (var productId in productIds)
        {
            // Check if the product detail is already exist for the product
            var dbProductDetails = await GetByProductIdAsync(productId);

            var duplicates = dbProductDetails.Select(x => x.Size.ToLower())
                .Intersect(productDetails.Where(x => x.ProductId == productId).Select(x => x.Size.ToLower())).ToList();

            if (duplicates.Count != 0)
            {
                string duplicateSizes = string.Join(",", duplicates);
                throw new Exception($"Product detail for Size ({duplicateSizes}) is already exist.");
            }

            var pDetails = productDetails.Where(x => x.ProductId == productId).ToList();

            await _db.ProductDetails.AddRangeAsync(pDetails);
        }

        await _db.SaveChangesAsync();

        return productDetails;
    }

    public async Task<List<Models.ProductDetail>> UpdateAsync(List<Models.ProductDetail> productDetails)
    {
        #region Check Duplicate

        var productIds = productDetails.Select(x => x.ProductId).Distinct().ToList();

        foreach (var productId in productIds)
        {
            // Check if the product detail is already exist for the product
            var dbProductDetails = await GetByProductIdAsync(productId);

            var productDetailIds = productDetails.Where(x => x.ProductId == productId).Select(x => x.Id).ToList();

            var duplicates = dbProductDetails.Where(x => !productDetailIds.Contains(x.Id)).Select(x => x.Size.ToLower())
                .Intersect(productDetails.Where(x => x.ProductId == productId).Select(x => x.Size.ToLower())).ToList();

            if (duplicates.Count != 0)
            {
                string duplicateSizes = string.Join(",", duplicates);
                throw new Exception($"Product detail for Size ({duplicateSizes}) is already exist.");
            }
        }

        #endregion

        foreach (var productDetail in productDetails)
        {
            var pDetail = await GetAsync(productDetail.Id);
            if (pDetail is not null)
            {
                pDetail.Price = productDetail.Price;
                pDetail.Size = productDetail.Size;
                pDetail.Design = productDetail.Design;
                pDetail.LastUpdatedOn = DateTime.Now;

                _db.ProductDetails.UpdateRange(pDetail);
            }
        }

        await _db.SaveChangesAsync();

        return productDetails;
    }

    public async Task<Models.ProductDetail?> GetAsync(Guid id)
    {
        return await _db.FindAsync<Models.ProductDetail>(id);
    }

    public async Task<List<Models.ProductDetail>> GetAsync()
    {
        return await _db.ProductDetails.ToListAsync();
    }

    public async Task<List<Models.ProductDetail>> GetByProductIdAsync(Guid productId)
    {
        return await _db.ProductDetails.Where(x => x.ProductId == productId).ToListAsync();
    }

    public async Task DeleteAsync(Guid productId)
    {
        var productDetails = await GetByProductIdAsync(productId);
        if (productDetails?.Count == 0)
        {
            throw new KeyNotFoundException("Product detail(s) not found");
        }

        _db.ProductDetails.RemoveRange(productDetails);
        await _db.SaveChangesAsync();
    }
}