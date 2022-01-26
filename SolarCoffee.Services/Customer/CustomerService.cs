using Microsoft.EntityFrameworkCore;
using SolarCoffee.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SolarCoffee.Services.Customer
{
    public class CustomerService : ICustomerService
    {
        private readonly SolarDbContext _db; 

        public CustomerService(SolarDbContext solarDbContext)
        {
            _db = solarDbContext;
        }

        /// <summary>
        /// Creating a new Customer
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public ServiceResponse<Data.Models.Customer> CreateCustomer(Data.Models.Customer customer)
        {
            try
            {
                _db.Customers.Add(customer);
                _db.SaveChanges();
                return new ServiceResponse<Data.Models.Customer>
                {
                   IsSuccess = true,
                   Message = "New Customer Created",
                   Time = DateTime.UtcNow,
                   Data = customer
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<Data.Models.Customer>
                {
                    IsSuccess= false,
                    Message = ex.StackTrace,
                    Time = DateTime.UtcNow,
                    Data= customer
                };
            }
            
        }
        /// <summary>
        /// Returns list of Customers from database
        /// </summary>
        /// <returns></returns>
        public List<Data.Models.Customer> GetAllCustomers()
        {
            return _db.Customers
                .Include(customer => customer.PrimaryAddress)
                .OrderBy(customer => customer.LastName)
                .ToList();
        }

        /// <summary>
        /// Gets a customer record by customerId
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Data.Models.Customer GetCustomer(int customerId)
        {
            return _db.Customers.Find(customerId);
        }

        /// <summary>
        /// Delete a customer record 
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        public ServiceResponse<bool> DeleteCustomer(int customerId)
        {
            var customer = _db.Customers.Find(customerId);
           if(customer == null)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess  = false,
                    Time = DateTime.UtcNow,
                    Message = "Customer to delete not found!",
                    Data = false
                };
            }
            try
            {
                _db.Customers.Remove(customer);
                _db.SaveChanges();
                return new ServiceResponse<bool>
                {
                    IsSuccess = true,
                    Time = DateTime.UtcNow,
                    Message = "Customer Deleted!",
                    Data = true
                };
            }
            catch (Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    IsSuccess = false,
                    Time = DateTime.UtcNow,
                    Message = ex.StackTrace,
                    Data = false
                };
            }           
            
        }


        
    }
}
