using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SolarCoffee.Services.Product;
using SolarCoffee.Web.Serialization;
using System.Linq;

namespace SolarCoffee.Web.Controllers
{
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;

        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet("api/product")]
        public ActionResult GetProduct()
        {
            _logger.LogInformation("Get all Products");
            var products = _productService.GetAllProducts();
            var productViewModel = products.Select(product => ProductMapper.SerializationProductMapper(product));
            return Ok(productViewModel);
        }
    }
}

