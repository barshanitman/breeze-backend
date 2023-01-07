using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_engine.Models;

public partial class TearSheetOutput:IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    [ForeignKey(nameof(FinancialYearId))]
    public int FinancialYearId{ get; set; }

    public string SheetReference { get; set; } = null!;

    public string CellReference { get; set; } = null!;

    public List<StockTearSheetOutput> StockTearSheetOutputs { get; set; } = new List<StockTearSheetOutput>();

}
