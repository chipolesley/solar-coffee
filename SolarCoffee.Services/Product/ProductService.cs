using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Product
{
    public class ProductService : IProductService
    {
        private readonly SolarDbContext _db;

        public ProductService(SolarDbContext dbContext)
        {
            _db = dbContext;
        }
        /// <summary>
        /// Retrieves All products from Database
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Product> GetAllProducts()
        {
            return _db.Products.ToList();
        }

        /// <summary>
        /// Retrieves Product form Databse by Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductById(int productId)
        {
            return _db.Products.Find(productId);
        }

        /// <summary>
        /// Adds new product to the Database
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> CreateProduct(Data.Models.Product product)
        {
            try
            {
                _db.Products.Add(product);

                var newInventory = new ProductInventory
                {
                    Product = product,
                    QuantityOnHand = 0,
                    IdealQuantity = 10

                };
                _db.ProductInventories.Add(newInventory);

                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = "Product saved Succefully...",
                    Data = product
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                    Message = $"Error in saving a new Product. {ex.StackTrace}",
                    Data = product
                };
            }

        }

        /// <summary>
        /// Retrieves Product from Database by Name
        /// </summary>
        /// <param name="productName"></param>
        /// <returns></returns>
        public Data.Models.Product GetProductByName(string productName)
        {
            return _db.Products.Find(productName);
        }

        /// <summary>
        /// Archives Product by setting a boolean IsArchived to true
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Product> ArchiveProduct(int productId)
        {
            try
            {
                var product = _db.Products.Find(productId);
                product.IsArchived = true;
                _db.SaveChanges();

                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = "Product Archived Successfully...",
                    Data = product
                };

            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Product>
                {
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                    Message = ex.StackTrace,
                    Data = null
                };
            }

        }
    }
}
