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
            var products = _productService.GetAllProducts();

            var productViewModel = products.Select(ProductMapper.SerializationProductMapper);
            return Ok(productViewModel);
        }
        [HttpPatch("api/product/{id}")]
        public ActionResult ArchiveProduct(int id)
        {
            _logger.LogInformation("Archiving a Product");
            var archivedProduct = _productService.ArchiveProduct(id);
            return Ok(archivedProduct);
        }
    }
}

