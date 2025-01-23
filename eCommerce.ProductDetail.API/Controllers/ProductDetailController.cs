using AutoMapper;
using eCommerce.ProductDetail.API.Models.Dtos;
using eCommerce.ProductDetail.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.ProductDetail.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductDetailController : ControllerBase
    {
        private readonly IProductDetailRepository _productDetailRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<ProductDetailController> _logger;

        private readonly ResponseDto _response;

        public ProductDetailController(IProductDetailRepository productDetailRepository,
            IMapper mapper,
            ILogger<ProductDetailController> logger)
        {
            _productDetailRepository = productDetailRepository;
            _mapper = mapper;
            _logger = logger;

            _response = new ResponseDto();
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var productDetails = await _productDetailRepository.GetAsync();
            _response.Result = _mapper.Map<IEnumerable<ProductDetailDto>>(productDetails);

            return Ok(_response);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Get(Guid id)
        {
            if (id == Guid.Empty)
            {
                _logger?.LogInformation("Product detail id is null or empty.");
                throw new BadHttpRequestException("Product detail id is null or empty.");
            }

            var productDetail = await _productDetailRepository.GetAsync(id);
            if (productDetail is null)
            {
                _logger?.LogInformation($"Product detail not found");
                throw new KeyNotFoundException($"Product detail not found");
            }

            _response.Result = _mapper.Map<ProductDetailDto>(productDetail);

            return Ok(_response);
        }

        [HttpGet]
        [Route("GetByProductId/{productId:Guid}")]
        public async Task<IActionResult> GetByProductId(Guid productId)
        {
            if (productId == Guid.Empty)
            {
                _logger?.LogInformation("Product id is null or empty.");
                throw new BadHttpRequestException("Product id is null or empty.");
            }

            var productDetails = await _productDetailRepository.GetByProductIdAsync(productId);
            if (!productDetails.Any())
            {
                _logger?.LogInformation($"Product details not found for Product id ({productId})");
                throw new KeyNotFoundException($"Product details not found");
            }

            _response.Result = _mapper.Map<IEnumerable<ProductDetailDto>>(productDetails);

            return Ok(_response);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] List<AddProductDetailRequestDto> request)
        {
            if (request is null)
            {
                _logger?.LogInformation("Product detail request is null or empty.");
                throw new BadHttpRequestException("Product detail request is null or empty.");
            }

            if (request.Any(x => x.ProductId == Guid.Empty))
            {
                _logger?.LogInformation("One or more product detail's product id is null or empty.");
                throw new BadHttpRequestException("One or more product detail's product id is null or empty.");
            }

            if (request.Any(x => x.Price <= 0d))
            {
                _logger?.LogInformation("One or more product detail is having price less than or equal to 0.");
                throw new BadHttpRequestException("One or more product detail is having price less than or equal to 0.");
            }

            var duplicateProductDetails = request.GroupBy(x => new { x.ProductId, x.Size })
                .Select(x => new { x.Key.Size, Count = x.Count() })
                .Where(x => x.Count > 1).ToList();

            if (duplicateProductDetails.Count != 0)
            {
                var duplicateSizes = string.Join(", ", duplicateProductDetails.Select(x => x.Size));
                _logger?.LogInformation($"A duplicate item has been detected. Each product detail must have a unique size ({duplicateSizes}).");
                throw new BadHttpRequestException($"A duplicate item has been detected. Each product detail must have a unique size ({duplicateSizes}).");
            }

            var productDetails = _mapper.Map<List<Models.ProductDetail>>(request);
            var result = await _productDetailRepository.AddAsync(productDetails);

            _response.Result = _mapper.Map<List<ProductDetailDto>>(result);

            return Ok(_response);
        }

        [HttpPut]
        public async Task<IActionResult> Put([FromBody] List<UpdateProductDetailRequestDto> request)
        {
            if (request is null)
            {
                _logger?.LogInformation("Product detail request is null or empty.");
                throw new BadHttpRequestException("Product detail request is null or empty.");
            }

            if (request.Any(x => x.Id == Guid.Empty))
            {
                _logger?.LogInformation("One or more product detail's id is null or empty.");
                throw new BadHttpRequestException("One or more product detail's id is null or empty.");
            }

            if (request.Any(x => x.ProductId == Guid.Empty))
            {
                _logger?.LogInformation("One or more product detail's product id is null or empty.");
                throw new BadHttpRequestException("One or more product detail's product id is null or empty.");
            }

            if (request.Any(x => x.Price <= 0d))
            {
                _logger?.LogInformation("Price should be greater than 0.");
                throw new BadHttpRequestException("Price should be greater than 0.");
            }

            var duplicateProductDetails = request.GroupBy(x => new { x.ProductId, x.Size })
                .Select(x => new { x.Key.Size, Count = x.Count() })
                .Where(x => x.Count > 1).ToList();

            if (duplicateProductDetails.Count != 0)
            {
                var duplicateSizes = string.Join(", ", duplicateProductDetails.Select(x => x.Size));
                _logger?.LogInformation($"A duplicate item has been detected. Each product detail must have a unique size ({duplicateSizes}).");
                throw new BadHttpRequestException($"A duplicate item has been detected. Each product detail must have a unique size ({duplicateSizes}).");
            }

            var productDetails = _mapper.Map<List<Models.ProductDetail>>(request);
            var result = await _productDetailRepository.UpdateAsync(productDetails);

            _response.Result = _mapper.Map<List<ProductDetailDto>>(result);

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

            await _productDetailRepository.DeleteAsync(productId);

            return Ok(_response);
        }
    }
}