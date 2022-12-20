using System;
using System.Collections.Generic;

namespace backend_engine.Models;

public partial class TearSheetOutput:IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<StockTearSheetOutput> StockTearSheetOutputs { get; } = new List<StockTearSheetOutput>();
}
