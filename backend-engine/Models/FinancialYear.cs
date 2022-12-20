using System;
using System.Collections.Generic;

namespace backend_engine.Models;

public partial class FinancialYear:IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int CalendarYear { get; set; }

    public bool Active { get; set; }

    public virtual ICollection<StockTearSheetOutput> StockTearSheetOutputs { get; } = new List<StockTearSheetOutput>();
}
