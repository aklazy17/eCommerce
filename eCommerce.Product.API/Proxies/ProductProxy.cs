using AutoMapper;
using eCommerce.Product.API.Data;
using eCommerce.Product.API.Models.Dtos;
using eCommerce.Product.API.Repositories;
using eCommerce.Product.API.Services;
using Microsoft.EntityFrameworkCore;

namespace eCommerce.Product.API.Proxies;

public class ProductProxy : IProductProxy
{
    private readonly IProductRepository _productRepository;
    private readonly IInventoryService _inventoryService;
    private readonly IProductDetailService _productDetailService;
    private readonly IMapper _mapper;
    private readonly ILogger<ProductProxy> _logger;

    public ProductProxy(IServiceProvider serviceProvider,
        IInventoryService inventoryService,
        IProductDetailService productDetailService,
        IMapper mapper,
        ILogger<ProductProxy> logger)
    {
        var dbContext = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());
        _productRepository = new ProductRepository(dbContext);
        _inventoryService = inventoryService;
        _productDetailService = productDetailService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ProductDto> AddAsync(AddProductRequestDto dto)
    {
        var productDto = new ProductDto();

        var product = _mapper.Map<Models.Product>(dto);

        // Create Product
        var result = await _productRepository.AddAsync(product);
        if (result is not null)
        {
            productDto = _mapper.Map<ProductDto>(result);

            #region Create Inventory

            try
            {
                var inventoryDto = new InventoryDto() { ProductId = result.Id, Quantity = dto.Quantity };

                // Create Inventory
                var inventory = await _inventoryService.AddAsync(inventoryDto);
                if (inventory is not null)
                {
                    productDto.InventoryInfo = inventory;
                }
            }
            catch (Exception ex)
            {
                // Rollback
                await _productRepository.DeleteAsync(productDto.Id);

                _logger?.LogInformation("Rollback");
                _logger?.LogError($"{ex}");

                throw new Exception(ex.Message);
            }

            #endregion

            #region Create Product Details

            try
            {
                var productDetailDto = _mapper.Map<List<ProductDetailDto>>(dto.ProductDetails);

                productDetailDto.ForEach(x => x.ProductId = result.Id);

                // Create Product Detail
                var productDetails = await _productDetailService.AddAsync(productDetailDto);
                if (productDetails is not null)
                {
                    productDto.ProductDetails = productDetails;
                }
            }
            catch (Exception ex)
            {
                // Rollback
                await _productRepository.DeleteAsync(productDto.Id);
                await _inventoryService.DeleteAsync(productDto.Id);

                _logger?.LogInformation("Rollback");
                _logger?.LogError($"{ex}");

                throw new Exception(ex.Message);
            }

            #endregion
        }

        return productDto;
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
        await _productDetailService.DeleteAsync(id);
        await _inventoryService.DeleteAsync(id);
    }

    public async Task<ProductDto?> GetAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id) ?? throw new KeyNotFoundException($"Product not found");

        var dto = _mapper.Map<ProductDto>(product);

        #region Get Inventory

        try
        {
            var inventory = await _inventoryService.GetByProductIdAsync(id);
            if (inventory is not null)
            {
                dto.InventoryInfo = inventory;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{ex}");

            throw new Exception(ex.Message);
        }

        #endregion

        #region Get Product Details

        try
        {
            var productDetails = await _productDetailService.GetByProductIdAsync(id);
            if (productDetails is not null)
            {
                dto.ProductDetails = productDetails;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{ex}");

            throw new Exception(ex.Message);
        }

        #endregion

        return dto;
    }

    public async Task<ProductDto?> GetAsync(string name)
    {
        var product = await _productRepository.GetAsync(name) ?? throw new KeyNotFoundException($"Product not found");

        var dto = _mapper.Map<ProductDto>(product);

        #region Get Inventory Info

        try
        {
            var inventory = await _inventoryService.GetByProductIdAsync(product.Id);
            if (inventory is not null)
            {
                dto.InventoryInfo = inventory;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{ex}");

            throw new Exception(ex.Message);
        }

        #endregion

        #region Get Product Details

        try
        {
            var productDetails = await _productDetailService.GetByProductIdAsync(product.Id);
            if (productDetails is not null)
            {
                dto.ProductDetails = productDetails;
            }
        }
        catch (Exception ex)
        {
            _logger?.LogError($"{ex}");

            throw new Exception(ex.Message);
        }

        #endregion

        return dto;
    }

    public async Task<List<ProductDto>> GetAsync()
    {
        var products = await _productRepository.GetAsync();
        var dtos = _mapper.Map<List<ProductDto>>(products);
        if (dtos.Count != 0)
        {
            var inventories = await _inventoryService.GetAsync();
            var productDetails = await _productDetailService.GetAsync();

            foreach (var product in dtos)
            {
                product.InventoryInfo = inventories?.FirstOrDefault(x => x.ProductId == product.Id);
                product.ProductDetails = productDetails?.Where(x => x.ProductId == product.Id).ToList();
            }
        }

        return dtos;
    }

    public async Task<ProductDto> UpdateAsync(UpdateProductRequestDto dto)
    {
        var productDto = new ProductDto();

        var product = _mapper.Map<Models.Product>(dto);

        // Update Product
        var result = await _productRepository.UpdateAsync(product);
        if (result is not null)
        {
            productDto = _mapper.Map<ProductDto>(result);

            #region Update Product Details

            try
            {
                var productDetailDto = _mapper.Map<List<ProductDetailDto>>(dto.ProductDetails);

                productDetailDto.ForEach(x => x.ProductId = result.Id);

                // Update Product Detail
                var productDetails = await _productDetailService.UpdateAsync(productDetailDto);
                if (productDetails is not null)
                {
                    productDto.ProductDetails = productDetails;
                }
            }
            catch (Exception ex)
            {
                _logger?.LogInformation("Rollback");
                _logger?.LogError($"{ex}");

                throw new Exception(ex.Message);
            }

            #endregion
        }

        return productDto;
    }
}