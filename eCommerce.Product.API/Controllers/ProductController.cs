using AutoMapper;
using eCommerce.Product.API.Models.Dtos;
using eCommerce.Product.API.Proxies;
using eCommerce.RabbitMq.Producers;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Product.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private const string RABBIT_MQ_ROUTING_KEY = "eComm-Notification";

        private readonly IProductProxy _productProxy;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductController> _logger;
        private readonly IMessageProducer _messageProducer;

        private readonly ResponseDto _response;

        public ProductController(IProductProxy productProxy,
            IMapper mapper,
            ILogger<ProductController> logger,
            IMessageProducer messageProducer)
        {
            _productProxy = productProxy;
            _mapper = mapper;
            _logger = logger;
            _messageProducer = messageProducer;

            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var products = await _productProxy.GetAsync();
            _response.Result = _mapper.Map<IEnumerable<ProductDto>>(products);

            return Ok(_response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger?.LogInformation("Product id is null or empty.");
                throw new BadHttpRequestException("Product id is null or empty.");
            }

            var product = await _productProxy.GetAsync(id);
            if (product is null)
            {
                _logger?.LogInformation($"Product not found");
                throw new KeyNotFoundException($"Product not found");
            }

            _response.Result = _mapper.Map<ProductDto>(product);

            return Ok(_response);
        }

        [HttpGet]
        [Route("{name}")]
        public async Task<IActionResult> Get(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                _logger?.LogInformation("Product name is null or empty.");
                throw new BadHttpRequestException("Product name is null or empty.");
            }

            var product = await _productProxy.GetAsync(name);
            if (product is null)
            {
                _logger?.LogInformation($"Product not found");
                throw new KeyNotFoundException($"Product not found");
            }

            _response.Result = _mapper.Map<ProductDto>(product);

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddProductRequestDto request)
        {
            if (request is null)
            {
                _logger?.LogInformation("Product request is null or empty.");
                throw new BadHttpRequestException("Product request is null or empty.");
            }

            if (request.ProductDetails is null || request.ProductDetails.Count == 0)
            {
                _logger?.LogInformation("Product details is null or empty.");
                throw new BadHttpRequestException("Product details is null or empty.");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                _logger?.LogInformation("Product name is null or empty.");
                throw new BadHttpRequestException("Product name is null or empty.");
            }

            var product = await _productProxy.AddAsync(request);
            _response.Result = product;

            try
            {
                await _messageProducer.SendMessageAsync(
                    new NotificationDto
                    {
                        EventType = Enums.EventType.ProductAdded,
                        Message = $"A new product has been added to the stock: {request.Name}.",
                        Recipient = "admin@ecommerce.com"
                    }, RABBIT_MQ_ROUTING_KEY);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex}");
            }

            return Ok(_response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] UpdateProductRequestDto request)
        {
            if (request is null)
            {
                _logger?.LogInformation("Product request is null or empty.");
                throw new BadHttpRequestException("Product request is null or empty.");
            }

            if (request.Id == Guid.Empty)
            {
                _logger?.LogInformation("Product id is null or empty.");
                throw new BadHttpRequestException("Product id is null or empty.");
            }

            if (request.ProductDetails is null || request.ProductDetails.Count == 0)
            {
                _logger?.LogInformation("Product detail is null or empty.");
                throw new BadHttpRequestException("Product detail is null or empty.");
            }

            if (string.IsNullOrEmpty(request.Name))
            {
                _logger?.LogInformation("Product name is null or empty.");
                throw new BadHttpRequestException("Product name is null or empty.");
            }

            _response.Result = await _productProxy.UpdateAsync(request);

            return Ok(_response);
        }

        [HttpDelete]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger?.LogInformation("Product id is null or empty.");
                throw new BadHttpRequestException("Product id is null or empty.");
            }

            await _productProxy.DeleteAsync(id);

            return Ok(_response);
        }
    }
}