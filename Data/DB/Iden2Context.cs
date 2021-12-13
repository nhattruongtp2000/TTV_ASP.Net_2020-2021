using System;
using Data.Configurations;
using Data.Data;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;


namespace Data.DB
{
    public partial class Iden2Context : IdentityDbContext<AppUser>
    {

        public Iden2Context(DbContextOptions<Iden2Context> options)
            : base(options)
        {
        }

        public DbSet<IpAccess> IpAccesses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Order> Orders { get; set; }

        public DbSet<EmailPromotion> EmailPromotions { get; set; }

        public DbSet<Category> Categories { get; set; }

        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<Slide> Slides { get; set; }

        public DbSet<Feedback> Feedbacks { get; set; }
        public DbSet<Contact> Contacts { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<ProductPhoto> ProductPhotos { get; set; }

        public DbSet<Voucher> Vouchers { get; set; }
        public DbSet<Blog> Blogs { get; set; }



        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())   //loại đi chữ aspnet
            {
                var tableName = entityType.GetTableName();
                if (tableName.StartsWith("AspNet"))
                {
                    entityType.SetTableName(tableName.Substring(6));
                }
            }
            modelBuilder.ApplyConfiguration(new OrderDetailsConfig());

            modelBuilder.Entity<Product>().HasIndex(x => x.Alias).IsUnique();

        }






    }
}
