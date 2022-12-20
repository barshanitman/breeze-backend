using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace backend_engine.Models;

public partial class BreezeDataContext : DbContext
{
    public BreezeDataContext()
    {
    }

    public BreezeDataContext(DbContextOptions<BreezeDataContext> options)
        : base(options)
    {
    }

    public virtual DbSet<FinancialYear> FinancialYears { get; set; }

    public virtual DbSet<ProfitLossDriver> ProfitLossDrivers { get; set; }

    public virtual DbSet<Stock> Stocks { get; set; }

    public virtual DbSet<StockTearSheetOutput> StockTearSheetOutputs { get; set; }

    public virtual DbSet<StockUpload> StockUploads { get; set; }

    public virtual DbSet<TearSheetOutput> TearSheetOutputs { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=tcp:breeze.database.windows.net,1433;Initial Catalog=BreezeData;Persist Security Info=False;User ID=breezeadmin;Password=!Breeze27;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<FinancialYear>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Financia__3214EC0724EC7555");

            entity.ToTable("FinancialYear");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<ProfitLossDriver>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__ProfitLo__3214EC076E863F51");

            entity.Property(e => e.InputCellReference)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InputName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.InputSheetReference)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OutputCellReference)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OutputName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.OutputSheetReference)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.StockUpload).WithMany(p => p.ProfitLossDrivers)
                .HasForeignKey(d => d.StockUploadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__ProfitLos__Stock__5D60DB10");
        });

        modelBuilder.Entity<Stock>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Stock__3214EC077A4CEF04");

            entity.ToTable("Stock");

            entity.Property(e => e.Code)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<StockTearSheetOutput>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StockTea__3214EC07D9553C62");

            entity.ToTable("StockTearSheetOutput");

            entity.Property(e => e.CellReference)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.SheetReference)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.FinancialYear).WithMany(p => p.StockTearSheetOutputs)
                .HasForeignKey(d => d.FinancialYearId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockTear__Finan__5A846E65");

            entity.HasOne(d => d.StockUpload).WithMany(p => p.StockTearSheetOutputs)
                .HasForeignKey(d => d.StockUploadId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockTear__Stock__59904A2C");

            entity.HasOne(d => d.TearSheetOutput).WithMany(p => p.StockTearSheetOutputs)
                .HasForeignKey(d => d.TearSheetOutputId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockTear__TearS__589C25F3");
        });

        modelBuilder.Entity<StockUpload>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__StockUpl__3214EC079A5B8454");

            entity.ToTable("StockUpload");

            entity.Property(e => e.FileName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.UploadedAt)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");

            entity.HasOne(d => d.Stock).WithMany(p => p.StockUploads)
                .HasForeignKey(d => d.StockId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__StockUplo__Stock__51EF2864");
        });

        modelBuilder.Entity<TearSheetOutput>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__TearShee__3214EC07220F1DF4");

            entity.ToTable("TearSheetOutput");

            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
