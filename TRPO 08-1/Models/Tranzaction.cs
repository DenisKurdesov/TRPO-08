using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;

namespace TRPO_08_1.Models
{
    public class Tranzaction
    {
        [Key]
        public int Id { get; set; }

        public string PaymentData { get; set; }

        public virtual List<Sale> Sales { get; set; }
    }

    public class TranzactionMap : EntityTypeConfiguration<Tranzaction>
    {
        public TranzactionMap()
        {
            this.HasMany(t => t.Sales)
                .WithRequired(s => s.Tranzaction)
                .WillCascadeOnDelete(false);
        }
    }
}