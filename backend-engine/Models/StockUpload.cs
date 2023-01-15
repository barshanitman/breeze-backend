using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
namespace backend_engine.Models;

public partial class StockUpload:IBaseEntity
{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime? UploadedAt { get; set; } = DateTime.UtcNow;

    [ForeignKey(nameof(StockId))]
    public int StockId { get; set; }

    public List<ProfitLossDriver> ProfitLossDrivers { get; set; } = new List<ProfitLossDriver>();

    public List<StockTearSheetOutput> StockTearSheetOutputs { get; set; } = new List<StockTearSheetOutput>();

    public List<DriverTearSheetOutput> DriverTearSheetOutputs { get; set; } = new List<DriverTearSheetOutput>();

    public List<StockUploadComment>  StockUploadComments{ get; set; } = new List<StockUploadComment>();
}
