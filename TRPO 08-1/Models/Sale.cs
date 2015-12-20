using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace TRPO_08_1.Models
{
    public class Sale
    {
        [Key]
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int MenuItemStockId { get; set; }
        public DateTime CreatedAt { get; set; }
        public int OrderStatusId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }
        public int? TranzactionId { get; set; }
        public int? OrderId { get; set; }

        [ForeignKey("CustomerId")]
        public virtual Customer Customer { get; set; }
        [ForeignKey("MenuItemStockId")]
        public virtual MenuItemStock MenuItemStock { get; set; }
        [ForeignKey("TranzactionId")]
        public virtual Tranzaction Tranzaction { get; set; }

        public virtual MenuItem MenuItem
        {
            get { return MenuItemStock.MenuItem; }
        }
    }

    public class SaleMap : EntityTypeConfiguration<Sale>
    {
        public SaleMap()
        {
            this.HasRequired(s => s.MenuItemStock);
        }
    }
}