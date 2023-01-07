using System;
using backend_engine.Models;

namespace backend_engine.DTOs
{

	public class StockDto
	{
	 public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Code { get; set; } = null!;

    public virtual ICollection<StockUploadDto>? StockUploads { get;  } = new List<StockUploadDto>();

	}
}

