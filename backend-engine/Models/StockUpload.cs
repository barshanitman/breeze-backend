using System;
using System.Collections.Generic;

namespace backend_engine.Models;

public partial class StockUpload:IBaseEntity
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime? UploadedAt { get; set; }

    public int StockId { get; set; }

    public virtual ICollection<ProfitLossDriver> ProfitLossDrivers { get; } = new List<ProfitLossDriver>();

    public virtual Stock Stock { get; set; } = null!;

    public virtual ICollection<StockTearSheetOutput> StockTearSheetOutputs { get; } = new List<StockTearSheetOutput>();
}
