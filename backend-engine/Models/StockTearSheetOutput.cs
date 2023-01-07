using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_engine.Models;

public partial class StockTearSheetOutput:IBaseEntity
{
    public int Id { get; set; }

    [ForeignKey(nameof(TearSheetOutputId))]
    public int TearSheetOutputId { get; set; }


    [ForeignKey(nameof(StockUploadId))]
    public int StockUploadId { get; set; }

   
   


}
