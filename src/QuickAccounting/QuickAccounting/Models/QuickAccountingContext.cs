using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using QuickAccounting.Data.Inventory;

namespace QuickAccounting.Models
{
    public partial class QuickAccountingContext : DbContext
    {
        public QuickAccountingContext()
        {
        }

        public QuickAccountingContext(DbContextOptions<QuickAccountingContext> options)
            : base(options)
        {
        }

        

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=10.5.2.221;Database=QuickAccounting;User ID=SFATestUser;Password=57Twq5TBR8PFJAJv;Trusted_Connection=false;TrustServerCertificate=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PurchaseOrderDetail>(entity =>
            {
                entity.HasKey(e => e.PurchaseOrderDetailsId);

                entity.Property(e => e.Amount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Discount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.DiscountAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GrossAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NetAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Qty).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Rate).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TaxRate).HasColumnType("decimal(18, 2)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
