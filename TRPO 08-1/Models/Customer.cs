using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using TRPO_08_1.Domain;

namespace TRPO_08_1.Models
{
    public class Customer
    {
        private static readonly List<string> ADDRESSES = new List<string>()
        {
            "Minsk Vostochnaya {0}-{1}",
            "Minsk Golodeda {0}-{1}",
            "Minsk Meleja {0}-{1}",
            "Minsk Nezavisimosti {0}-{1}",
            "Minsk Kalinovskogo {0}-{1}",
            "Minsk Pobeditelei {0}-{1}",
            "Minsk Kolasa {0}-{1}",
        };

        public Customer() { }

        public Customer(string email, string name, string passwordHash, CustomerRole role, Random random)
        {
            this.Email = email;
            this.Name = name;
            this.Address = string.Format(ADDRESSES[random.Next(ADDRESSES.Count - 1)], random.Next(200) + 1, random.Next(200) + 1);
            this.PasswordHash = passwordHash;
            this.Phone = string.Format("({0}) {1}-{2}-{3}", random.Next(99), random.Next(899) + 100, random.Next(89) + 10, random.Next(89) + 10);
            this.CustomerRoleId = (int)role;
        }

        [Key]
        public int Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }
        public int CustomerRoleId { get; set; }

        public virtual CustomerRole CustomerRole
        {
            get { return (CustomerRole) this.CustomerRoleId; }
            set { this.CustomerRoleId = (int) value; }
        }

        public virtual List<Sale> Sales { get; set; }

        public virtual List<Sale> ShoppingCartItems
        {
            get { return Sales != null 
                ? Sales.Where(s => !s.OrderId.HasValue).ToList()
                : new List<Sale>(); 
            }
        }

        public virtual List<int> OrderIds
        {
            get
            {
                return PaidItems.Concat(UnpaidItems)
                    .Where(s => s.OrderId.HasValue)
                    .Select(s => s.OrderId.GetValueOrDefault())
                    .Distinct()
                    .ToList();
            }
        }

        public virtual List<Sale> PaidItems
        {
            get
            {
                return Sales != null
                    ? Sales.Where(s => s.OrderId.HasValue && s.TranzactionId.HasValue).ToList()
                    : new List<Sale>();
            }
        }
        
        public virtual List<Sale> UnpaidItems
        {
            get
            {
                return Sales != null
                    ? Sales.Where(s => s.OrderId.HasValue && !s.TranzactionId.HasValue).ToList()
                    : new List<Sale>();
            }
        }

        public virtual List<Sale> OrderItems(int orderId)
        {
            return Sales != null
                ? Sales.Where(s => s.OrderId == orderId).ToList()
                : new List<Sale>();
        }
    }

    public class CustomerMap : EntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            this.HasMany(c => c.Sales)
                .WithRequired(s => s.Customer)
                .WillCascadeOnDelete();
            
        }
    }
}