﻿using System;

namespace SolarCoffee.Web.ViewModels
{
    /// <summary>
    /// Product entity data transfer Object (DTO). 
    /// </summary>
    public class ProductViewModel
    {        
        public int Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool IsTaxable { get; set; }
        public bool IsArchived { get; set; }
    }
}
