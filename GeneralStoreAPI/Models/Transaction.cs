using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace GeneralStoreAPI.Models
{
  
    
        public class Transaction
        {
            [Key]
            public int ID { get; set; }

            [ForeignKey(nameof(Customer.ID))]
            public int CustomerID { get; set; }

            [ForeignKey(nameof(Product.SKU))]
            public string ProductSKU { get; set; }

            public int ItemCount { get; set; }

            public DateTime DateOfTransaction { get; set; }

            

        }
    
}