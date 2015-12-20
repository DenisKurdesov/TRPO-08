using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TRPO_08_1.Models
{
    public class MenuItem
    {
        public MenuItem() { }

        public MenuItem(string name, string description)
        {
            this.Name = name;
            this.Description = description;
        }

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public virtual List<Product> Products { get; set; }

        public virtual List<MenuItemStock> MenuItemsStock { get; set; }
    }

    public class MenuItemMap : EntityTypeConfiguration<MenuItem>
    {
        public MenuItemMap()
        {
            this.HasMany(mi => mi.Products)
                .WithMany(p => p.MenuItems)
                .Map(mc =>
                {
                    mc.ToTable("MenuItemProductMapping");
                    mc.MapLeftKey("MenuItemId");
                    mc.MapRightKey("ProductId");
                }
            );

            this.HasMany(mi => mi.MenuItemsStock)
                .WithRequired(mis => mis.MenuItem);
        }
    }
}