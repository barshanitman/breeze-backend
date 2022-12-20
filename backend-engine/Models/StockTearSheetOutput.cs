using System;
using System.Collections.Generic;

namespace backend_engine.Models;

public partial class StockTearSheetOutput:IBaseEntity
{
    public int Id { get; set; }

    public int TearSheetOutputId { get; set; }

    public int StockUploadId { get; set; }

    public int FinancialYearId { get; set; }

    public string SheetReference { get; set; } = null!;

    public string CellReference { get; set; } = null!;

    public virtual FinancialYear FinancialYear { get; set; } = null!;

    public virtual StockUpload StockUpload { get; set; } = null!;

    public virtual TearSheetOutput TearSheetOutput { get; set; } = null!;
}
