using static CalculatorModels.Responses;

namespace CalculatorModels
{
	public class ErrorList
	{
        public static Error e400 = new Error()
        {
            ErrorCode = "BadRequest",
            ErrorStatus = 400,
            ErrorMessage = "Unable to process request: ..."
        };
    

        public static Error e500 = new Error()
        {
            ErrorCode = "InternalError",
            ErrorStatus = 500,
            ErrorMessage = "An unexpected error condition was triggered which made impossible to fulfill the request. Please try again or contact support."
        };
	}
}
