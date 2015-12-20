using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TRPO_08_1.Models
{
    public class Restaraunt
    {
        public Restaraunt() { }

        public Restaraunt(string name, string address, Customer manager)
        {
            Name = name;
            Address = address;
            ManagerId = manager.Id;
        }

        [Key]
        public int Id { get; set; }
        public int ManagerId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        [ForeignKey("ManagerId")]
        [DataType("Customer")]
        public virtual Customer Manager { get; set; }

        public virtual List<Sale> Sales { get; set; }
        public virtual List<MenuItemStock> MenuItemsStock { get; set; }
        public virtual List<ProductStock> ProductsStock { get; set; }
    }

    public class RestarauntMap : EntityTypeConfiguration<Restaraunt>
    {
        public RestarauntMap()
        {
            this.HasRequired(r => r.Manager)
                .WithMany()
                .WillCascadeOnDelete(false);
            this.HasMany(r => r.MenuItemsStock)
                .WithRequired(mis => mis.Restaraunt)
                .WillCascadeOnDelete();
            this.HasMany(r => r.ProductsStock)
                .WithRequired(ps => ps.Restaraunt)
                .WillCascadeOnDelete();
        }
    }
}