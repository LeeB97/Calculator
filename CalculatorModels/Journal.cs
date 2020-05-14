using System;
using System.Collections.Generic;

namespace CalculatorModels
{
	public class JournalRequest
	{
		public string Id { get; set; }
	}

	public class JournalResponse
	{
		public string Id { get; set; }
		public List<Registry> Operations = new List<Registry>();
	}

	public class Registry
	{
		public string Operation { get; set; }
		public string Calculation { get; set; }
		public DateTime Date { get; set; }
	}
}
