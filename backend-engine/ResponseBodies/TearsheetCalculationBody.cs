using System;
namespace backend_engine.ResponseBodies
{
	public class TearsheetCalculationBody
	{
		public List<object> totalRevenue { get; set; }
		public List<object> cogs { get; set; }
		public List<object>  grossProfit { get; set; }
		public List<object>  opex { get; set; }
		public List<object>  other { get; set; }
		public List<object>  ebitda { get; set; }
		public List<object>  dandA { get; set; }
		public List<object>  ebit { get; set; }
		public List<object>  netInterest { get; set; }
		public List<object>  netTax { get; set; }
		public List<object>  npat { get; set; }
		public List<object>  sharesonIssue { get; set; }
		public List<object>  eps { get; set; }
		public List<object>  dividendsDistributed { get; set; }
		public List<object>  operatingCF { get; set; }
		public List<object>  investingCF { get; set; }
		public List<object>  financingCashFlow { get; set; }
		public List<object>  netCashFlowInPeriod { get; set; }
		public List<object>  closingCashBalance { get; set; }
		public List<object>  totalCurrentAssets { get; set; }
		public List<object>  totalCurrentLiabilities { get; set; }
		public List<object>  totalEquity { get; set; }
		public List<object>  totalBorrowings { get; set; }
		public List<object>  otherInclusionsToNetDebt { get; set; }
		public List<object>  netDebtOverCash { get; set; }
		public List<object>  totalRevenueGrowth { get; set; }
		public List<object>  ebitdaGrowth { get; set; }
		public List<object>  ebitGrowth { get; set; }
		public List<object>  npatMargin { get; set; }
		public List<object>  enterpriseValue { get; set; }
		public List<object>  evOverRevenue { get; set; }
		public List<object>	 evOverEbitda { get; set; }
		public List<object>  evOverEbit { get; set; }
		public List<object>  pOverE{ get; set; }



	}
}

