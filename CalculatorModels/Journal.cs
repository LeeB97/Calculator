namespace CalculatorModels
{
	public class Journal
	{
		public string Id { get; set; }
		public Registry[] Operations { get; set; }
	}

	public class Registry
	{
		public string Operation { get; set; }
		public string Calculation { get; set; }
		public string Date { get; set; }
	}
}
