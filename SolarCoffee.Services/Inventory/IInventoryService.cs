using SolarCoffee.Data.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SolarCoffee.Services.Inventory
{
    public interface IInventoryService
    {
       public List<ProductInventory> GetCurrentInventory();
       public ServiceResponse<ProductInventory> UpdateInventory(int id, int adjustment);
       public ProductInventory GetByProductId(int productId);
       public List<ProductInventorySnapshot> GetInventorySnapShotHistory();
    }
}
