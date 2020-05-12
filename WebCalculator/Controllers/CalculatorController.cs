using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Results;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
			ErrorMessage = "Unable to process request: ..."
		};

		[HttpPost]
		public JsonResult<string> add(Requests.Add request)
		{
			var response = new Responses.Add();
			response.Sum = request.Addends.Sum();

			return Json("{'Sum': " + response.Sum + "}");
		}

		[HttpPost]
		public JsonResult<string> subtract(Requests.Sub request)
		{

			var ctx = new ValidationContext(request);
			var errors = new List<ValidationResult>();
			var isValid = Validator.TryValidateObject(request, ctx, errors, true);

			if (!isValid)
			{
				throw new AggregateException(
					errors.Select((e) => new ValidationException("error"))
				);
			}

			var response = new Responses.Sub();
			response.Difference = request.Minuend - request.Subtrahend;

			return Json("exito");
		}

		[HttpPost]
		public IHttpActionResult multiply(Requests.Mult request)
		{
			var response = new Responses.Mult();
			response.Product = request.Factors.Aggregate(1, (a, b) => a * b);

			return Json(response);
		}

		[HttpPost]
		public IHttpActionResult divide(Requests.Div request)
		{
			var response = new Responses.Div();
			int remainder;
			response.Quotient = Math.DivRem(request.Dividend, request.Divisor, out remainder);
			response.Remainder = remainder;

			return Json(response);
		}

		[HttpPost]
		public IHttpActionResult squareRoot(Requests.Sqrt request)
		{
			var response = new Responses.Sqrt();
			response.Square = Math.Sqrt(Convert.ToDouble(request.Number));

			return Json(response);
		}
	}
}
