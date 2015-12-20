using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TRPO_08_1.Models
{
    public class MenuItemStock
    {
        public MenuItemStock() { }

        public MenuItemStock(MenuItem menuItem, Restaraunt restaraunt, int price, int quantity)
        {
            this.MenuItem = menuItem;
            this.Restaraunt = restaraunt;
            this.Price = price;
            this.Quantity = quantity;
        }

        [Key]
        public int Id { get; set; }
        public int MenuItemId { get; set; }
        public int RestarauntId { get; set; }
        public int Price { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("MenuItemId")]
        public virtual MenuItem MenuItem { get; set; }
        [ForeignKey("RestarauntId")]
        public virtual Restaraunt Restaraunt { get; set; }
    }

    public class MenuItemStockMap : EntityTypeConfiguration<MenuItemStock>
    {
        public MenuItemStockMap()
        {
            this.HasRequired(mis => mis.MenuItem)
                .WithMany();
        }
    }
}