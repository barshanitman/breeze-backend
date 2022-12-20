using System;
using System.Collections.Generic;

namespace backend_engine.Models;

public partial class ProfitLossDriver:IBaseEntity
{
    public int Id { get; set; }

    public int StockUploadId { get; set; }

    public string InputName { get; set; } = null!;

    public string InputSheetReference { get; set; } = null!;

    public string InputCellReference { get; set; } = null!;

    public string OutputName { get; set; } = null!;

    public string OutputSheetReference { get; set; } = null!;

    public string OutputCellReference { get; set; } = null!;

    public virtual StockUpload StockUpload { get; set; } = null!;
}
