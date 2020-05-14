using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using CalculatorModels;

namespace WebCalculator.Controllers
{
    public class CalculatorController : ApiController
	{
		private readonly Responses.Error _400Error = new Responses.Error()
		{
			ErrorCode = "Bad Request",
			ErrorStatus = 400,
			ErrorMessage = "Unable to process request: ..."
		};

		private readonly Responses.Error _500Error = new Responses.Error()
		{
			ErrorCode = "InternalError",
			ErrorStatus = 500,
			ErrorMessage = "Error at processing the request."
		};

		[HttpPost]
		public IHttpActionResult add(Requests.Add request)
		{
			if (request == null || request.Addends == null || request.Addends.Length < 2)
				return Content(HttpStatusCode.BadRequest, _400Error);

			var response = new Responses.Add();
			response.Sum = request.Addends.Sum();

			if(response == null || response.Sum == null)
				return Content(HttpStatusCode.InternalServerError, _500Error);

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult subtract(Requests.Sub request)
		{
			if (request == null || request.Minuend == null || request.Subtrahend == null)
					return Content(HttpStatusCode.BadRequest, _400Error);

			var response = new Responses.Sub();
			response.Difference = request.Minuend - request.Subtrahend;

			if (response == null || response.Difference == null)
				return Content(HttpStatusCode.InternalServerError, _500Error);

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult multiply(Requests.Mult request)
		{
			if (request == null || request.Factors == null || request.Factors.Length < 2)
				return Content(HttpStatusCode.BadRequest, _400Error);

			var response = new Responses.Mult();
			response.Product = request.Factors.Aggregate((a, b) => a * b);

			if (response == null || response.Product == null)
				return Content(HttpStatusCode.InternalServerError, _500Error);

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult divide(Requests.Div request)
		{
			if (request == null || request.Dividend == null || request.Divisor == null)
				return Content(HttpStatusCode.BadRequest, _400Error);

			var response = new Responses.Div();
			var remainder = 0;
			response.Quotient = Math.DivRem((int)request.Dividend, (int)request.Divisor, out remainder);
			response.Remainder = remainder;

			if (response == null || response.Quotient == null || response.Remainder == null)
				return Content(HttpStatusCode.InternalServerError, _500Error);

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult squareRoot(Requests.Sqrt request)
		{
			if (request == null || request.Number == null)
				return Content(HttpStatusCode.BadRequest, _400Error);

			var response = new Responses.Sqrt();
			response.Square = Math.Sqrt(Convert.ToDouble(request.Number));

			if (response == null || response.Square == null)
				return Content(HttpStatusCode.InternalServerError, _500Error);

			return Ok(response);
		}
	}
}
