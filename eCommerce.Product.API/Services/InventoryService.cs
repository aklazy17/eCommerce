using eCommerce.Product.API.Models.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace eCommerce.Product.API.Services;

public class InventoryService : IInventoryService
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly ILogger<InventoryService> _logger;

    public InventoryService(IHttpClientFactory httpClientFactory,
        ILogger<InventoryService> logger)
    {
        _httpClientFactory = httpClientFactory;
        _logger = logger;
    }

    public async Task<InventoryDto?> AddAsync(InventoryDto inventory)
    {
        InventoryDto? inventoryDto = null;

        var client = _httpClientFactory.CreateClient("Inventory");

        var inventoryJson = JsonConvert.SerializeObject(new { inventory.ProductId, inventory.Quantity });
        var requestContent = new StringContent(inventoryJson, Encoding.UTF8, "application/json");

        var response = await client.PostAsync($"/api/inventory", requestContent);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                inventoryDto = JsonConvert.DeserializeObject<InventoryDto>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return inventoryDto;
    }

    public async Task<InventoryDto?> AddQuantityByProductIdAsync(Guid productId, int quantity)
    {
        InventoryDto? inventoryDto = null;

        var client = _httpClientFactory.CreateClient("Inventory");

        var response = await client.PutAsync($"/api/inventory/quantity/{productId}/{quantity}/add", null);
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                inventoryDto = JsonConvert.DeserializeObject<InventoryDto>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return inventoryDto;
    }

    public async Task DeleteAsync(Guid productId)
    {
        var client = _httpClientFactory.CreateClient("Inventory");

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

    public async Task<List<InventoryDto>?> GetAsync()
    {
        List<InventoryDto>? inventories = null;

        var client = _httpClientFactory.CreateClient("Inventory");

        var response = await client.GetAsync($"/api/inventory");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                inventories = JsonConvert.DeserializeObject<List<InventoryDto>>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return inventories;
    }

    public async Task<InventoryDto?> GetByProductIdAsync(Guid productId)
    {
        InventoryDto? inventory = null;

        var client = _httpClientFactory.CreateClient("Inventory");

        var response = await client.GetAsync($"/api/inventory/{productId}");
        if (response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<ResponseDto>(content);
            if (result != null && result.IsSuccess)
            {
                inventory = JsonConvert.DeserializeObject<InventoryDto>(Convert.ToString(result.Result));
            }
            else
            {
                _logger.LogInformation(result?.Message);
                throw new Exception(result?.Message);
            }
        }

        return inventory;
    }
}