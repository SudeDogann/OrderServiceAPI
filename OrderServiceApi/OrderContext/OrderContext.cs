using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using Order.Entity;


namespace OrderContext
{
    public class OrderContext : DbContext
    {
        public OrderContext(DbContextOptions<OrderContext> options) : base(options)
        {
        }

        public OrderContext()
        {
        }

        public DbSet<Order.Entity.Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(@"Server=SUDE\SQLEXPRESS;Database=OrderDb;Trusted_Connection=True;");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("dbo");

            modelBuilder.Entity<Order.Entity.Order>(entity =>
            {
                entity.ToTable("Order");

                entity.Property(i => i.Id).HasColumnType("int").UseIdentityColumn().IsRequired();
                entity.Property(i => i.StoreName).HasMaxLength(100);
                entity.Property(i => i.CustomerName).HasMaxLength(100);
            });

            base.OnModelCreating(modelBuilder);
        }

    }

}
