using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Inventory
{
    public class InventoryService : IInventoryService
    {
        private readonly SolarDbContext _db;
        private readonly ILogger<InventoryService> _logger;

        public InventoryService(SolarDbContext solarDbContext, ILogger<InventoryService> logger)
        {
            _db = solarDbContext;
            _logger = logger;
        }

        /// <summary>
        /// Get product instance by product Id
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public ProductInventory GetByProductId(int productId)
        {
            return _db.ProductInventories
                .Include(p => p.Product)
                .FirstOrDefault(pi => pi.Product.Id == productId);
        }

        /// <summary>
        /// Return all Current Inventory from database.
        /// </summary>
        /// <returns></returns>
        public List<ProductInventory> GetCurrentInventory()
        {
            return _db.ProductInventories
                    .Include(p => p.Product)
                    .Where(pi => !pi.Product.IsArchived)
                    .ToList();
        }

        /// <summary>
        /// Adjust QuantityOnHand on Current Inventory
        /// </summary>
        /// <param name="Id">productId</param>
        /// <param name="adjustment">Number of units added / removed from inventory</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ServiceResponse<ProductInventory> UpdateInventory(int id, int adjustment)
        {
            try
            {
                var inventory = _db.ProductInventories
                    .Include(inv => inv.Product)
                    .First(inv => inv.Product.Id == id);
                
                inventory.QuantityOnHand += adjustment;
                

                try
                {
                    CreateSnapShot(inventory);
                    _db.SaveChanges();
                }
                catch (Exception ex)
                {
                    _logger.LogError("There was an error on creating inventory SnapShot");
                    _logger.LogError(ex.StackTrace);
                }

                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = true,
                    Message = $"Product {id} was updated by {adjustment} units",
                    Time = DateTime.UtcNow,
                    Data = inventory
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<ProductInventory>
                {
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = null
                };
            }
            
        }

        /// <summary>
        /// Return Snapshot history for previous 6 hours
        /// </summary>
        /// <returns></returns>
        public List<ProductInventorySnapshot> GetInventorySnapShotHistory()
        {
            var earliest = DateTime.UtcNow - TimeSpan.FromHours(6);
            return _db.ProductInventorySnapshots
                .Include(snap => snap.Product)
                .Where(snap => snap.SnapshotTime > earliest && !snap.Product.IsArchived)
                .ToList();
        }

        /// <summary>
        /// Creates a snapshot record using the provided InventoryProduct instance
        /// </summary>
        /// <param name="inventory"></param>
        private void CreateSnapShot(ProductInventory inventory)
        {
            var snapShot = new ProductInventorySnapshot
            {
                SnapshotTime = DateTime.UtcNow,
                Product = inventory.Product,
                QuantityOnHand = inventory.QuantityOnHand
            };
            _db.Add(snapShot);
        }

        
    }
}
