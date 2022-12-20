using System;
using System.Collections.Generic;

namespace backend_engine.Models;

public partial class Stock:IBaseEntity
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<StockUpload> StockUploads { get; } = new List<StockUpload>();
}
