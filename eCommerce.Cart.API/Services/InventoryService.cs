using eCommerce.Cart.API.Models.Dtos;
using Newtonsoft.Json;
using System.Text;

namespace eCommerce.Cart.API.Services;

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

    public async Task<InventoryDto?> UpdateQuantityByProductIdAsync(Guid productId, int quantity)
    {
        InventoryDto? inventoryDto = null;

        var client = _httpClientFactory.CreateClient("Inventory");

        var response = await client.PutAsync($"/api/inventory/quantity/{productId}/{quantity}/update", null);
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
}