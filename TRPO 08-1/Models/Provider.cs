using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Web;

namespace TRPO_08_1.Models
{
    public class Provider
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Info { get; set; }
    }

    public class ProviderMap : EntityTypeConfiguration<Provider>
    {
        public ProviderMap()
        {


        }
    }
}