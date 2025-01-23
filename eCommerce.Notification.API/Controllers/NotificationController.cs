using AutoMapper;
using eCommerce.Notification.API.Models.Dtos;
using eCommerce.Notification.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.Notification.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<NotificationController> _logger;

        private readonly ResponseDto _response;

        public NotificationController(INotificationRepository notificationRepository,
            IMapper mapper,
            ILogger<NotificationController> logger)
        {
            _notificationRepository = notificationRepository;
            _mapper = mapper;
            _logger = logger;

            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var notifications = await _notificationRepository.GetAsync();
            _response.Result = _mapper.Map<IEnumerable<NotificationDto>>(notifications);

            return Ok(_response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger?.LogInformation("Notification id is null or empty.");
                throw new BadHttpRequestException("Notification id is null or empty.");
            }

            var notification = await _notificationRepository.GetAsync(id);
            if (notification is null)
            {
                _logger?.LogInformation($"Notification not found");
                throw new KeyNotFoundException($"Notification not found");
            }

            _response.Result = _mapper.Map<NotificationDto>(notification);

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AddNotificationRequestDto request)
        {
            if (request is null)
            {
                _logger?.LogInformation("Notification request is null or empty.");
                throw new BadHttpRequestException("Notification request is null or empty.");
            }

            if (string.IsNullOrEmpty(request.Message))
            {
                _logger?.LogInformation("Message is null or empty.");
                throw new BadHttpRequestException("Message is null or empty.");
            }

            var notifications = _mapper.Map<Models.Notification>(request);
            var result = await _notificationRepository.AddAsync(notifications);

            _response.Result = _mapper.Map<NotificationDto>(result);

            return Ok(_response);
        }
    }
}