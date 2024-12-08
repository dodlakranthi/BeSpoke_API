using BeSpokedBikesAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BeSpokedBikesAPI.DataAccess
{
    public class BeSpokedBikesSQLContainer : DbContext
    {
        public BeSpokedBikesSQLContainer(DbContextOptions<BeSpokedBikesSQLContainer> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Customers>()
                .HasKey(x => x.CustomerId);

            modelBuilder.Entity<SalesPersons>()
                .HasKey(x => x.SalesPersonId);

            modelBuilder.Entity<Products>()
                .HasKey(x => x.ProductId);

            modelBuilder.Entity<Sales>()
                .HasKey(x => x.SaleId);

            modelBuilder.Entity<Discounts>()
                .HasKey(x => x.DiscountId);
        }

        public DbSet<Products> Product { get; set; }
        public DbSet<SalesPersons> SalesPerson { get; set; }
        public DbSet<Customers> Customer { get; set; }
        public DbSet<Sales> Sale { get; set; }
        public DbSet<Discounts> Discount { get; set; }
    }
}
