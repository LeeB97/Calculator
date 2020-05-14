namespace CalculatorModels
{
	public class ErrorList
	{
		public Responses.Error get400()
		{
			return new Responses.Error()
			{
				ErrorCode = "InternalError",
				ErrorStatus = 400,
				ErrorMessage = "Bad request, wrong information."
			};
		}

		public Responses.Error get500()
		{
			return new Responses.Error()
			{
				ErrorCode = "InternalError",
				ErrorStatus = 500,
				ErrorMessage = "An unexpected error condition was triggered which made impossible to fulfill the request."
			};
		}
	}
}
