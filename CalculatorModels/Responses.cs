namespace CalculatorModels
{
	public class Responses
	{
		public class Add
		{
			public int? Sum { get; set; }
		}

		public class Sub
		{
			public int? Difference { get; set; }
		}

		public class Mult
		{
			public int? Product { get; set; }
		}

		public class Div
		{
			public int? Quotient { get; set; }
			public int? Remainder { get; set; }
		}

		public class Sqrt
		{
			public double? Square { get; set; }
		}

		public class Error : IResponse
		{
			public string ErrorCode { get; set; }
			public int ErrorStatus { get; set; }
			public string ErrorMessage { get; set; }
		}
	}
}
