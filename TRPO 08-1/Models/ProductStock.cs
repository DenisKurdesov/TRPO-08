using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TRPO_08_1.Models
{
    public class ProductStock
    {
        [Key]
        [Column(Order = 0)]
        public int ProductId { get; set; }
        [Key]
        [Column(Order = 1)]
        public int RestarauntId { get; set; }
        public int Quantity { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("RestarauntId")]
        public virtual Restaraunt Restaraunt { get; set; }
    }

    public class ProductStockMap : EntityTypeConfiguration<ProductStock>
    {
        public ProductStockMap()
        {
            this.HasRequired(ps => ps.Product);
            this.HasRequired(ps => ps.Restaraunt)
                .WithMany(r => r.ProductsStock)
                .WillCascadeOnDelete();
        }
    }
}