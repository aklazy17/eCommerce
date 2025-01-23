using AutoMapper;
using eCommerce.Inventory.API.Models.Dtos;
using eCommerce.Inventory.API.Repositories;
using eCommerce.RabbitMq.Producers;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Inventory.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InventoryController : ControllerBase
{
    private const int LOW_STOCK_THRESHOLD = 5;
    private const string RABBIT_MQ_ROUTING_KEY = "eComm-Notification";

    private readonly IInventoryRepository _inventoryRepository;
    private readonly IMapper _mapper;
    private readonly ILogger<InventoryController> _logger;
    private readonly IMessageProducer _messageProducer;

    private readonly ResponseDto _response;

    public InventoryController(IInventoryRepository inventoryRepository,
        IMapper mapper,
        ILogger<InventoryController> logger,
        IMessageProducer messageProducer)
    {
        _inventoryRepository = inventoryRepository;
        _mapper = mapper;
        _logger = logger;
        _messageProducer = messageProducer;

        _response = new ResponseDto();
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var inventories = await _inventoryRepository.GetAsync();
        _response.Result = _mapper.Map<IEnumerable<InventoryDto>>(inventories);

        return Ok(_response);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddInventoryRequestDto request)
    {
        if (request is null)
        {
            _logger?.LogInformation("Inventory request is null or empty.");
            throw new BadHttpRequestException("Inventory request is null or empty.");
        }

        // Add Product Validation
        if (request.ProductId == Guid.Empty)
        {
            _logger?.LogInformation("Product id is null or empty.");
            throw new BadHttpRequestException("Product id is null or empty.");
        }

        var inventory = _mapper.Map<Models.Inventory>(request);
        var result = await _inventoryRepository.AddAsync(inventory);

        _response.Result = _mapper.Map<InventoryDto>(result);

        return Ok(_response);
    }

    [HttpGet]
    [Route("{productId:Guid}")]
    public async Task<IActionResult> Get(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            _logger?.LogInformation("Product id is null or empty.");
            throw new BadHttpRequestException("Product id is null or empty.");
        }

        var inventory = await _inventoryRepository.GetByProductIdAsync(productId);
        if (inventory == null)
        {
            _logger?.LogInformation($"Inventory not found for Product id ({productId})");
            throw new KeyNotFoundException($"Inventory not found");
        }

        _response.Result = _mapper.Map<InventoryDto>(inventory);

        return Ok(_response);
    }

    [HttpPut]
    [Route("quantity/{productId:Guid}/{quantity:int}/add")]
    public async Task<IActionResult> AddQuantity(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
        {
            _logger?.LogInformation("Product id is null or empty.");
            throw new BadHttpRequestException("Product id is null or empty.");
        }

        if (quantity <= 0)
        {
            _logger?.LogInformation("Quantity should be greater than 0.");
            throw new BadHttpRequestException("Quantity should be greater than 0.");
        }

        var result = await _inventoryRepository.AddQuantityByProductIdAsync(productId, quantity);

        _response.Result = _mapper.Map<InventoryDto>(result);

        return Ok(_response);
    }

    [HttpPut]
    [Route("quantity/{productId:Guid}/{quantity:int}/update")]
    public async Task<IActionResult> UpdateQuantity(Guid productId, int quantity)
    {
        if (productId == Guid.Empty)
        {
            _logger?.LogInformation("Product id is null or empty.");
            throw new BadHttpRequestException("Product id is null or empty.");
        }

        if (quantity <= 0)
        {
            _logger?.LogInformation("Quantity should be greater than 0.");
            throw new BadHttpRequestException("Quantity should be greater than 0.");
        }

        var result = await _inventoryRepository.UpdateQuantityByProductIdAsync(productId, quantity);
        var inventoryDto = _mapper.Map<InventoryDto>(result);
        _response.Result = inventoryDto;

        if (inventoryDto.Quantity <= LOW_STOCK_THRESHOLD)
        {
            try
            {
                // Low Stock Notification
                await _messageProducer.SendMessageAsync(
                    new NotificationDto
                    {
                        EventType = Enums.EventType.ProductAdded,
                        Message = $"Stock is running low. Product Id: {inventoryDto.ProductId}",
                        Recipient = "user@testmail.com"
                    }, RABBIT_MQ_ROUTING_KEY);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex}");
            }
        }

        return Ok(_response);
    }

    [HttpDelete]
    [Route("{productId:Guid}")]
    public async Task<IActionResult> Delete(Guid productId)
    {
        if (productId == Guid.Empty)
        {
            _logger?.LogInformation("Product id is null or empty.");
            throw new BadHttpRequestException("Product id is null or empty.");
        }

        await _inventoryRepository.DeleteAsync(productId);

        return Ok(_response);
    }
}