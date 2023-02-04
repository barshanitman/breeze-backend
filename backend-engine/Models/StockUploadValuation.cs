using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend_engine.Models;

public partial class StockUploadValuation : IBaseEntity
{
    public int Id { get; set; }

    public string? Methodologies { get; set; }


    [ForeignKey(nameof(StockUploadId))]
    public int StockUploadId { get; set; }


}
