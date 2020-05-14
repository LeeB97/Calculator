namespace CalculatorModels
{
	public class Requests
	{
		public class Add : IRequest
		{
			public int[] Addends { get; set; }
		}

		public class Sub : IRequest
		{
			public int? Minuend { get; set; }
			public int? Subtrahend { get; set; }
		}

		public class Mult : IRequest
		{
			public int[] Factors { get; set; }
		}

		public class Div : IRequest
		{
			public int? Dividend { get; set; }
			public int? Divisor { get; set; }
		}

		public class Sqrt : IRequest
		{
			public int? Number { get; set; }
		}
	}
}
