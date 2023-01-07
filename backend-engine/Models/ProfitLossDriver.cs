using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_engine.Models;

public partial class ProfitLossDriver:IBaseEntity
{
    public int Id { get; set; }

    [ForeignKey(nameof(StockUploadId))]
    public int StockUploadId { get; set; }

    public string InputName { get; set; } 

    public string InputSheetReference { get; set; } 

    public string InputCellReference { get; set; } 

    public string OutputName { get; set; } 

    public string OutputSheetReference { get; set; } 

    public string OutputCellReference { get; set; } 


}
