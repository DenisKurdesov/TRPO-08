using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using TRPO_08_1.Domain;
using TRPO_08_1.Models;
using TRPO_08_1.Sevices;

namespace TRPO_08_1.Data
{
    public class RestarauntContext : DbContext
    {
        private const int _maxProductsPerMenuItem = 5;
        private const int _minMenuItemsPerRestaraunt = 5;
        private const int _maxMenuItemsPerRestaraunt = 15;
        private const int _minMenuItemPrice = 1000;
        private const int _maxMenuItemPrice = 100000;

        private readonly EncryptingService _encryptingService;
        private readonly Random _random;

        public RestarauntContext()
        {
            _encryptingService = new EncryptingService();
            _random = new Random();
        }

        public DbSet<Customer> Customers { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<MenuItemStock> MenuItemsStock { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductStock> ProductsStock { get; set; }
        public DbSet<Provider> Providers { get; set; }
        public DbSet<Restaraunt> Restaraunts { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Tranzaction> Tranzactions { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new CustomerMap());
            modelBuilder.Configurations.Add(new RestarauntMap());
            modelBuilder.Configurations.Add(new MenuItemMap());
            modelBuilder.Configurations.Add(new MenuItemStockMap());
            modelBuilder.Configurations.Add(new SaleMap());
            modelBuilder.Configurations.Add(new ProductStockMap());
            modelBuilder.Configurations.Add(new TranzactionMap());
            modelBuilder.Configurations.Add(new ProviderMap());


            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }

        public class CreateInitializer : DropCreateDatabaseAlways<RestarauntContext>
        {
            protected override void Seed(RestarauntContext context)
            {
                context.Seed();
                base.Seed(context);
            }
        }

        public void Seed()
        {
            int restarauntsCount = 3;

            SeedCustomers(10, restarauntsCount);
            SeedRestaraunts(restarauntsCount);
            SeedProducts();
            SeedMenuItems();

            this.SaveChanges();
        }

        public void SeedCustomers(int customersCount, int managersCount)
        {
            var customers = new List<Customer>()
            {
                new Customer("director@mail.com", "Денис", _encryptingService.GetPasswordHash("director"), CustomerRole.Director, _random),
                new Customer("admin@mail.com", "", _encryptingService.GetPasswordHash("admin"), CustomerRole.Admin, _random),
            };

            for (int i = 0; i < customersCount; i++)
            {
                customers.Add(new Customer(string.Format("user{0}@mail.com", i), "", _encryptingService.GetPasswordHash("user"), CustomerRole.User, _random));
            }

            for (int i = 0; i < managersCount; i++)
            {
                customers.Add(new Customer(string.Format("manager{0}@mail.com", i), "", _encryptingService.GetPasswordHash("manager"), CustomerRole.Manager, _random));
            }

            customers.ForEach(c => this.Customers.Add(c));

            this.SaveChanges();
        }

        public void SeedRestaraunts(int count)
        {
            var names = new string[] { "Bon Apeti ", "McDonalds", "Big Stack ", "Ponio", "Windows ", "Periano", "Lackucho" };
            var addresses = new string[] { "Минск", "Минск", "Минск", "Минск", "Минск", "Минск", "Минск" };

            var restaraunts = new List<Restaraunt>()
            {

            };

            for (int i = 0; i < count; i++)
            {
                var managerEmail = string.Format("manager{0}@mail.com", i);
                restaraunts.Add(new Restaraunt(names[i], addresses[i], this.Customers.First(c => c.Email == managerEmail)));
            }

            restaraunts.ForEach(r => this.Restaraunts.Add(r));

            this.SaveChanges();
        }

        public void SeedProducts()
        {
            var products = new List<Product>()
            {
                new Product("Говядина"),
                new Product("Курица"),
                new Product("Свинина"),
                new Product("Тесто"),
                new Product("Кетчуп"),
                new Product("Масло"),
                new Product("Огурец"),
                new Product("Томат"),
                new Product("Капуста"),
                new Product("Брокколи"),
            };

            products.ForEach(p => this.Products.Add(p));

            this.SaveChanges();
        }

        public void SeedMenuItems()
        {
            var products = this.Products.ToList();
            var restaraunts = this.Restaraunts.ToList();
            var menuItems = new List<MenuItem>()
            {
                new MenuItem("Пицца Техасская", "Очень вкусная пицца!"),
                new MenuItem("Пицца Терра", "Очень вкусная пицца!"),
                new MenuItem("Пицца Оскар", "Очень вкусная пицца!"),
                new MenuItem("Пицца с морепродуктами", "Очень вкусная пицца!"),
                new MenuItem("Пицца Таёжная ", "Очень вкусная пицца!"),
                new MenuItem("Пицца Калабрезе ", ""),
                new MenuItem("Пицца Капричиоза ", ""),
                new MenuItem("Пенне Буоно", ""),
                new MenuItem("Спагетти со шпинатом", ""),
                new MenuItem("Суп куриный", ""),
                new MenuItem("Суп с морепродуктами", ""),
                new MenuItem("Стейк из говядины", ""),
                new MenuItem("Свинина с драниками и грибами", ""),
                new MenuItem("Треска в кунжутной корочке", ""),
                new MenuItem("Стейк из сёмги", ""),
            };
            menuItems.ForEach(mi => this.MenuItems.Add(mi));
            menuItems.ForEach(mi =>
            {
                mi.Products = products.OrderBy(p => _random.Next()).Take(_random.Next(1, _maxProductsPerMenuItem)).ToList();
            });

            var menuItemsStock = new List<MenuItemStock>();
            foreach (var restaraunt in restaraunts)
            {
                var menuItemsForRestaraunt = menuItems.OrderBy(mi => _random.Next())
                    .Take(_random.Next(_minMenuItemsPerRestaraunt, _maxMenuItemsPerRestaraunt)).ToList();
                menuItemsStock.AddRange(menuItemsForRestaraunt.Select(mi => 
                    new MenuItemStock(mi, restaraunt, _random.Next(_minMenuItemPrice, _maxMenuItemPrice), _random.Next(10))));
            }
            menuItemsStock.ForEach(mis => this.MenuItemsStock.Add(mis));

            this.SaveChanges();
        }
    }
}