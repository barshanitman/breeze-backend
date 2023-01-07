using System;
using System.Collections.Generic;

namespace backend_engine.Models;

public partial class Stock:IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public DateTime? CreatedAt { get; set; } = DateTime.UtcNow;

    public string Code { get; set; } = null!;

    public List<StockUpload> StockUploads { get; set; } = new List<StockUpload>();

}
