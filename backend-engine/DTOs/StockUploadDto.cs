using System;
using backend_engine.Models;

namespace backend_engine.DTOs
{
	public class StockUploadDto
	{
    public int Id { get; set; }

    public string FileName { get; set; } = null!;

    public DateTime? UploadedAt { get; set; }

    public int StockId { get; set; }


	}
}

