using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using SolarCoffee.Data.Models;
using SolarCoffee.Services.Inventory;
using SolarCoffee.Services.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Order
{
    public class OrderService : IOrderService
    {
        private readonly SolarDbContext _db;
        private readonly IProductService _productService;
        private readonly IInventoryService _invetoryService;

        public OrderService(SolarDbContext solarDbContext, IProductService productService)
        {
            _db = solarDbContext;
            _productService = productService;
        }

        /// <summary>
        /// Create an open sales order
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public ServiceResponse<bool> GenerateOpenOrder(SalesOrder order)
        {
            foreach(var item in order.SalesOrderItems)
            {
                item.Product = _productService.GetProductById(item.Product.Id);
    
                var inventoryId = _invetoryService.GetByProductId(item.Product.Id).Id;
                _invetoryService.UpdateInventory(inventoryId, -item.Quantity);
            }
            try
            {
                _db.Add(order);
                _db.SaveChanges();

                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = "Open order created",
                    Time = DateTime.UtcNow,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow,
                    Data = false
                };

            }
            
        }

        /// <summary>
        /// Returns all Sales Orders in the system
        /// </summary>
        /// <returns></returns>
        public List<SalesOrder> GetAllOrders()
        {
            return _db.SalesOrders
                 .Include(c => c.Customer)
                    .ThenInclude(customer => customer.PrimaryAddress)
                 .Include(c => c.SalesOrderItems)
                    .ThenInclude(item => item.Product)
                 .ToList();
        }

        /// <summary>
        /// marks an open order as paid
        /// </summary>
        /// <param name="salesOrderId"></param>
        /// <returns></returns>
        public ServiceResponse<bool> MarkFulFilled(int salesOrderId)
        {
            var order = _db.SalesOrders.Find(salesOrderId);
            order.IsPaid = true;
            order.UpdatedOn = DateTime.UtcNow;
               
            try
            {
                _db.SalesOrders.Update(order);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Message = $"Order {order.Id} closed, Invoice paid in Full!",
                    Time = DateTime.UtcNow,
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Message = $"Order Not Found!. Error {ex.StackTrace}",
                    Time = DateTime.UtcNow,
                    Data = false
                };
            }                
        }
    }
}
