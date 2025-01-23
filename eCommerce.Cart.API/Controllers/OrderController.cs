using AutoMapper;
using eCommerce.Cart.API.Models;
using eCommerce.Cart.API.Models.Dtos;
using eCommerce.Cart.API.Repositories;
using eCommerce.RabbitMq.Producers;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Cart.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private const string RABBIT_MQ_ROUTING_KEY = "eComm-Notification";

        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<OrderController> _logger;
        private readonly IMessageProducer _messageProducer;

        private readonly ResponseDto _response;

        public OrderController(IOrderRepository orderRepository,
            IMapper mapper,
            ILogger<OrderController> logger,
            IMessageProducer messageProducer)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _logger = logger;
            _messageProducer = messageProducer;

            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var orders = await _orderRepository.GetAsync();

            _response.Result = _mapper.Map<List<OrderDto>>(orders);

            return Ok(_response);
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger?.LogInformation("Order id is null or empty.");
                throw new BadHttpRequestException("Order id is null or empty.");
            }

            var order = await _orderRepository.GetOrderAsync(id) ?? throw new KeyNotFoundException("Order not found.");

            _response.Result = _mapper.Map<OrderDto>(order);

            return Ok(_response);
        }

        [HttpPost]
        [Route("AddToCart/{orderId:guid?}")]
        public async Task<IActionResult> AddToCart(Guid? orderId, [FromBody] AddToCartRequestDto dto)
        {
            if (dto is null)
            {
                _logger?.LogInformation("Cart item is null or empty.");
                throw new BadHttpRequestException("Cart item is null or empty.");
            }

            if (dto.ProductId == Guid.Empty)
            {
                _logger?.LogInformation("Product id is null or empty.");
                throw new BadHttpRequestException("Product id is null or empty.");
            }

            if (dto.Quantity <= 0)
            {
                _logger?.LogInformation("Quantity should be greater than 0.");
                throw new BadHttpRequestException("Quantity should be greater than 0.");
            }

            var orderDetail = _mapper.Map<OrderDetail>(dto);
            var order = await _orderRepository.AddToCartAsync(orderId, orderDetail);

            _response.Result = _mapper.Map<OrderDto>(order);

            return Ok(_response);
        }

        [HttpDelete]
        [Route("RemoveCart/{orderId:guid}")]
        public async Task<IActionResult> RemoveCart(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                _logger?.LogInformation("Order id is null or empty.");
                throw new BadHttpRequestException("Order id is null or empty.");
            }

            await _orderRepository.RemoveCartAsync(orderId);

            return Ok(_response);
        }

        [HttpDelete]
        [Route("RemoveCartItem/{orderId:guid}/{orderDetailId:guid}")]
        public async Task<IActionResult> RemoveCartItem(Guid orderId, Guid orderDetailId)
        {
            if (orderId == Guid.Empty)
            {
                _logger?.LogInformation("Order id is null or empty.");
                throw new BadHttpRequestException("Order id is null or empty.");
            }

            if (orderDetailId == Guid.Empty)
            {
                _logger?.LogInformation("Order detail id is null or empty.");
                throw new BadHttpRequestException("Order detail id is null or empty.");
            }

            var order = await _orderRepository.RemoveCartItemAsync(orderId, orderDetailId);
            _response.Result = _mapper.Map<OrderDto>(order);

            return Ok(_response);
        }

        [HttpPut]
        [Route("Checkout/{orderId:Guid}")]
        public async Task<IActionResult> Checkout(Guid orderId)
        {
            if (orderId == Guid.Empty)
            {
                _logger?.LogInformation("Order id is null or empty.");
                throw new BadHttpRequestException("Order id is null or empty.");
            }

            var order = await _orderRepository.CheckoutAsync(orderId);
            var orderDto = _mapper.Map<OrderDto>(order);
            _response.Result = orderDto;

            try
            {
                await _messageProducer.SendMessageAsync(
                    new NotificationDto
                    {
                        EventType = Enums.EventType.ProductAdded,
                        Message = $"The order has been successfully checked out. Order Id: {orderDto.Id}",
                        Recipient = "user@testmail.com"
                    }, RABBIT_MQ_ROUTING_KEY);
            }
            catch (Exception ex)
            {
                _logger?.LogError($"{ex}");
            }

            return Ok(_response);
        }
    }
}