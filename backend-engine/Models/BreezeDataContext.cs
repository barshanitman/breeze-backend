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

    public DbSet<FinancialYear> FinancialYears { get; set; }

    public DbSet<ProfitLossDriver> ProfitLossDrivers { get; set; }

    public DbSet<Stock> Stocks { get; set; }

    public DbSet<StockTearSheetOutput> StockTearSheetOutputs { get; set; }

    public DbSet<StockUpload> StockUploads { get; set; }

    public DbSet<TearSheetOutput> TearSheetOutputs { get; set; }

    public DbSet<SummaryTearSheetOutput> SummaryTearSheetOutputs { get; set; }

    public DbSet<StockUploadTearsheetValue> StockUploadTearsheetValues {get;set;}

    public DbSet<DriverTearSheetOutput> DriverTearSheetOutputs { get; set; }



}
