﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using backend_engine.Models;

#nullable disable

namespace backendengine.Migrations
{
    [DbContext(typeof(BreezeDataContext))]
    partial class BreezeDataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("backend_engine.Models.DriverTearSheetOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CellReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FinancialYearId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFormula")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SheetReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockUploadId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("FinancialYearId");

                    b.HasIndex("StockUploadId");

                    b.ToTable("DriverTearSheetOutputs");
                });

            modelBuilder.Entity("backend_engine.Models.FinancialYear", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("FinancialYears");
                });

            modelBuilder.Entity("backend_engine.Models.ProfitLossDriver", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("InputCellReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InputName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("InputSheetReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputCellReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OutputSheetReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockUploadId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StockUploadId");

                    b.ToTable("ProfitLossDrivers");
                });

            modelBuilder.Entity("backend_engine.Models.Stock", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Stocks");
                });

            modelBuilder.Entity("backend_engine.Models.StockTearSheetOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("StockUploadId")
                        .HasColumnType("int");

                    b.Property<int>("TearSheetOutputId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StockUploadId");

                    b.HasIndex("TearSheetOutputId");

                    b.ToTable("StockTearSheetOutputs");
                });

            modelBuilder.Entity("backend_engine.Models.StockUpload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("StockId");

                    b.ToTable("StockUploads");
                });

            modelBuilder.Entity("backend_engine.Models.StockUploadComment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("StockUploadId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("UploadedAt")
                        .HasColumnType("datetime2");

                    b.Property<string>("comment")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("imageURL")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("type")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("StockUploadId");

                    b.ToTable("StockUploadComments");
                });

            modelBuilder.Entity("backend_engine.Models.StockUploadTearsheetValue", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("StockUploadId")
                        .HasColumnType("int");

                    b.Property<int>("TearSheetOutputId")
                        .HasColumnType("int");

                    b.Property<double>("Value")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.ToTable("StockUploadTearsheetValues");
                });

            modelBuilder.Entity("backend_engine.Models.StockUploadValuation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Methodologies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StockUploadId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StockUploadId")
                        .IsUnique();

                    b.ToTable("StockUploadValuations");
                });

            modelBuilder.Entity("backend_engine.Models.SummaryTearSheetOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CellReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SheetReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("SummaryTearSheetOutputs");
                });

            modelBuilder.Entity("backend_engine.Models.TearSheetOutput", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CellReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("FinancialYearId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SheetReference")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("FinancialYearId");

                    b.ToTable("TearSheetOutputs");
                });

            modelBuilder.Entity("backend_engine.Models.DriverTearSheetOutput", b =>
                {
                    b.HasOne("backend_engine.Models.FinancialYear", null)
                        .WithMany("DriverTearSheetOutputs")
                        .HasForeignKey("FinancialYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_engine.Models.StockUpload", null)
                        .WithMany("DriverTearSheetOutputs")
                        .HasForeignKey("StockUploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.ProfitLossDriver", b =>
                {
                    b.HasOne("backend_engine.Models.StockUpload", null)
                        .WithMany("ProfitLossDrivers")
                        .HasForeignKey("StockUploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.StockTearSheetOutput", b =>
                {
                    b.HasOne("backend_engine.Models.StockUpload", null)
                        .WithMany("StockTearSheetOutputs")
                        .HasForeignKey("StockUploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("backend_engine.Models.TearSheetOutput", null)
                        .WithMany("StockTearSheetOutputs")
                        .HasForeignKey("TearSheetOutputId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.StockUpload", b =>
                {
                    b.HasOne("backend_engine.Models.Stock", null)
                        .WithMany("StockUploads")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.StockUploadComment", b =>
                {
                    b.HasOne("backend_engine.Models.StockUpload", null)
                        .WithMany("StockUploadComments")
                        .HasForeignKey("StockUploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.StockUploadValuation", b =>
                {
                    b.HasOne("backend_engine.Models.StockUpload", null)
                        .WithOne("StockUploadValuation")
                        .HasForeignKey("backend_engine.Models.StockUploadValuation", "StockUploadId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.TearSheetOutput", b =>
                {
                    b.HasOne("backend_engine.Models.FinancialYear", null)
                        .WithMany("TearSheetOutputs")
                        .HasForeignKey("FinancialYearId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.FinancialYear", b =>
                {
                    b.Navigation("DriverTearSheetOutputs");

                    b.Navigation("TearSheetOutputs");
                });

            modelBuilder.Entity("backend_engine.Models.Stock", b =>
                {
                    b.Navigation("StockUploads");
                });

            modelBuilder.Entity("backend_engine.Models.StockUpload", b =>
                {
                    b.Navigation("DriverTearSheetOutputs");

                    b.Navigation("ProfitLossDrivers");

                    b.Navigation("StockTearSheetOutputs");

                    b.Navigation("StockUploadComments");

                    b.Navigation("StockUploadValuation")
                        .IsRequired();
                });

            modelBuilder.Entity("backend_engine.Models.TearSheetOutput", b =>
                {
                    b.Navigation("StockTearSheetOutputs");
                });
#pragma warning restore 612, 618
        }
    }
}
