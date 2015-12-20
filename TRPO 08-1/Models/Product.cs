using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TRPO_08_1.Models
{
    public class Product
    {
        public Product() {}
        
        public Product(string name)
        {
            this.Name = name;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public virtual List<MenuItem> MenuItems { get; set; }
    }
}