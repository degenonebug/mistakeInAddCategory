using ItemApiProject.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ItemApiProject.Services
{
    public class ItemDbContext : DbContext
    {
        public ItemDbContext(DbContextOptions<ItemDbContext> options)
            :base(options)
        {
            Database.Migrate();
        }

        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<Manufacturer> Manufacturers { get; set; }
        public virtual DbSet<Category> Categories { get; set; }
        public virtual DbSet<ItemManufacturer> ItemManufacturers { get; set; }
        public virtual DbSet<ItemCategory> ItemCategories { get; set; }

        protected override void OnModelCreating (ModelBuilder modelBuilder)
        {
            

            modelBuilder.Entity<ItemCategory>()
                        .HasKey(ic => new { ic.ItemId, ic.CategoryId });
            modelBuilder.Entity<ItemCategory>()
                        .HasOne(i => i.Item)
                        .WithMany(ic => ic.ItemCategories)
                        .HasForeignKey(i => i.ItemId);
            modelBuilder.Entity<ItemCategory>()
                        .HasOne(c => c.Category)
                        .WithMany(im => im.ItemCategories)
                        .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<ItemManufacturer>()
                        .HasKey(im => new { im.ItemId, im.ManufacturerId });
            modelBuilder.Entity<ItemManufacturer>()
                        .HasOne(i => i.Item)
                        .WithMany(ic => ic.ItemManufacturers)
                        .HasForeignKey(i => i.ItemId);
            modelBuilder.Entity<ItemManufacturer>()
                        .HasOne(c => c.Manufacturer)
                        .WithMany(im => im.ItemManufacturers)
                        .HasForeignKey(c => c.ManufacturerId);

        }
    }
}
