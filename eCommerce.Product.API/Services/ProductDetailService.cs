using eCommerce.Product.API.Models.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace eCommerce.Product.API.Services;

public class ProductDetailService : IProductDetailService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<ProductDetailService> _logger;

    public ProductDetailService(IHttpClientFactory httpClientFactory,
        ILogger<ProductDetailService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<List<ProductDetailDto>?> AddAsync(List<ProductDetailDto> productDetails)
    {
        List<ProductDetailDto>? productdetailsDto = null;

        var client = _httpClientFactory.CreateClient("ProductDetail");

        var productDetailsJson = JsonConvert.SerializeObject(productDetails);
        var requestContent = new StringContent(productDetailsJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"/api/productdetail", requestContent);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                productdetailsDto = JsonConvert.DeserializeObject<List<ProductDetailDto>>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return productdetailsDto;
    }

    public async Task<List<ProductDetailDto>?> GetAsync()
    {
        List<ProductDetailDto>? productDetails = null;

        var client = _httpClientFactory.CreateClient("ProductDetail");

        var response = await client.GetAsync($"/api/productdetail");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                productDetails = JsonConvert.DeserializeObject<List<ProductDetailDto>>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return productDetails;
    }

    public async Task<List<ProductDetailDto>?> GetByProductIdAsync(Guid productId)
    {
        List<ProductDetailDto>? productDetails = null;

        var client = _httpClientFactory.CreateClient("ProductDetail");

        var response = await client.GetAsync($"/api/productdetail/getbyproductid/{productId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                productDetails = JsonConvert.DeserializeObject<List<ProductDetailDto>>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return productDetails;
    }

    public async Task<List<ProductDetailDto>?> UpdateAsync(List<ProductDetailDto> productDetails)
    {
        List<ProductDetailDto>? productdetailsDto = null;

        var client = _httpClientFactory.CreateClient("ProductDetail");

        var productDetailsJson = JsonConvert.SerializeObject(productDetails);
        var requestContent = new StringContent(productDetailsJson, Encoding.UTF8, "application/json");

        var response = await client.PutAsync($"/api/productdetail", requestContent);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                productdetailsDto = JsonConvert.DeserializeObject<List<ProductDetailDto>>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return productdetailsDto;
    }

    public async Task DeleteAsync(Guid productId)
    {
        var client = _httpClientFactory.CreateClient("ProductDetail");

        var response = await client.DeleteAsync($"/api/delete/{productId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && !result.IsSuccess)
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }
    }
}