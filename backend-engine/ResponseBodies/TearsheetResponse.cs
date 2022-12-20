using System;
namespace backend_engine.ResponseBodies
{
	public class TearsheetResponse
	{
		public List<object> TotalRevenue { get; set; }
		public List<object> Cogs { get; set; }
		public List<object>  GrossProfit { get; set; }
		public List<object>  Opex { get; set; }
		public List<object> EBITDA { get; set; }
		public List<object> DandA { get; set; }
		public List<object>  EBIT { get; set; }
		public List<object> NetInterest { get; set;}
		public List<object> NetTax { get; set;}
		public List<object> NPAT { get; set;}
		public List<object> DividendsDistributed { get; set;}
		public List<object> OperatingCF { get; set;}
		public List<object> InvestingCF { get; set;}
		public List<object> FinancingCashFlow { get; set;}
		public List<object> NetCashFlowFromOperatingActivities { get; set;}
		public List<object> ClosingCashBalance { get; set;}
		public List<object> TotalBorrowings { get; set;}
		public List<object> TotalLeaseLiabilities { get; set;}
		public List<object> TotalRevenueGrowth { get; set;}
		public List<object> EBITDAGrowth { get; set;}
		public List<object> EBITGrowth { get; set;}
		public List<object> NPATGrowth { get; set;}
		public List<object> GrossMargin { get; set;}
		public List<object> EBITDAMargin { get; set;}
		public List<object> EBITMargin { get; set;}
		public List<object> NPATMargin { get; set;}
		public List<object> EVOverRevenue { get; set;}
		public List<object> EVOverEBITDA { get; set;}
		public List<object> EVOverEBIT { get; set;}
		public List<object> POverE { get; set;}
	}
}

