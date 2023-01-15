using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace backend_engine.Models
{
	public class StockUploadComment
	{
		public int Id { get; set; }

		[ForeignKey(nameof(StockUploadId))]
		public int StockUploadId { get; set; }

		public string type { get; set; } = "comment";

		public string comment { get; set; }

		public string imageURL { get; set; } = "https://images.unsplash.com/photo-1520785643438-5bf77931f493?ixlib=rb-=eyJhcHBfaWQiOjEyMDd9&auto=format&fit=facearea&facepad=8&w=256&h=256&q=8";


		public DateTime? UploadedAt { get; set; } = DateTime.UtcNow;




	}
}


