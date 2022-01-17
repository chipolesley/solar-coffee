using System;
using System.Collections.Generic;
using System.Text;

namespace SolarCoffee.Services.Product
{
    public interface IProductService
    {
        List<Data.Models.Product> GetAllProducts();
        Data.Models.Product GetProductById(int productId);
        Data.Models.Product GetProductByName(string productName);
        ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product);
        ServiceResponse<Data.Models.Product> ArchiveProduct(int productId);
    }
}
