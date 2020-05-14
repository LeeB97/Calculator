using System;
using System.Linq;
using System.Net;
using System.Web.Http;
using CalculatorModels;
using System.Collections.Generic;

namespace WebCalculator.Controllers
{
    public class CalculatorController : ApiController
	{
		#region privates
		private const string HeaderName = "X-Evi-Tracking-Id";
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger(); // longs in: \logs
		private readonly ErrorList error = new ErrorList();
		#endregion

		#region requests
		[HttpPost]
		public IHttpActionResult add(Requests.Add request)
		{
			logger.Info("Received Add Request");
			
			if (request == null || request.Addends == null || request.Addends.Length < 2)
			{
				logger.Debug($"Bad Add request: {request} - Addends: {request?.Addends}");
				return Content(HttpStatusCode.BadRequest, error.get400());
			}

			logger.Info("Processing Add Request");
			var response = new Responses.Add();
			response.Sum = request.Addends.Sum();

			if (response == null || response.Sum == null)
			{
				logger.Debug($"Internal error: request: {response} - Sum: {response?.Sum}");
				return Content(HttpStatusCode.InternalServerError, error.get500());
			}

			if (Request.Headers.Contains(HeaderName))
			{
				var headerValue = Request.Headers.GetValues(HeaderName).First();
				logger.Info("Header detected, Id: " + headerValue);

				var j = new JournalResponse();
				j.Id = headerValue;
				var o = new Registry();
				o.Operation = "Sum";
				o.Date = DateTime.UtcNow;
				o.Calculation = string.Join(" + ", request.Addends);
				j.Operations.ToList().Add(o);
				JournalController.JournalList.ToList().Add(j);
			}

			logger.Info("Sending Add Response");

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult subtract(Requests.Sub request)
		{
			logger.Info("Received Subtract Request");

			if (request == null || request.Minuend == null || request.Subtrahend == null)
			{
				logger.Debug($"Bad Subtract request: {request} - Minuend: {request?.Minuend}, Subtrahend: {request.Subtrahend}");
				return Content(HttpStatusCode.BadRequest, error.get400());
			}

			logger.Info("Processing Subtract Request");
			var response = new Responses.Sub();
			response.Difference = request.Minuend - request.Subtrahend;

			if (response == null || response.Difference == null)
			{
				logger.Debug($"Internal error: request: {response} - Difference: {response?.Difference}");
				return Content(HttpStatusCode.InternalServerError, error.get500());
			}

			logger.Info("Sending Subtract Response");

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult multiply(Requests.Mult request)
		{
			logger.Info("Received Multiply Request");

			if (request == null || request.Factors == null || request.Factors.Length < 2)
			{
				logger.Debug($"Bad Multiply request: {request} - Factors: {request?.Factors}");
				return Content(HttpStatusCode.BadRequest, error.get400());
			}

			logger.Info("Processing Multiply Request");
			var response = new Responses.Mult();
			response.Product = request.Factors.Aggregate((a, b) => a * b);

			if (response == null || response.Product == null)
			{
				logger.Debug($"Internal error: request: {response} - Factors: {response?.Product}");
				return Content(HttpStatusCode.InternalServerError, error.get500());
			}

			logger.Info("Sending Multiply Response");

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult divide(Requests.Div request)
		{
			logger.Info("Received Divide Request");

			if (request == null || request.Dividend == null || request.Divisor == null)
			{
				logger.Debug($"Bad Divide request: {request} - Dividend: {request?.Dividend}, Divisor: {request.Divisor}");
				return Content(HttpStatusCode.BadRequest, error.get400());
			}

			logger.Info("Processing Divide Request");
			var response = new Responses.Div();
			var remainder = 0;
			response.Quotient = Math.DivRem((int)request.Dividend, (int)request.Divisor, out remainder);
			response.Remainder = remainder;

			if (response == null || response.Quotient == null || response.Remainder == null)
			{
				logger.Debug($"Internal error: request: {response} - Quotient: {response?.Quotient}, Remainder: {response?.Remainder}");
				return Content(HttpStatusCode.InternalServerError, error.get500());
			}

			logger.Info("Sending Divide Response");

			return Ok(response);
		}

		[HttpPost]
		public IHttpActionResult squareRoot(Requests.Sqrt request)
		{
			logger.Info("Received Square Root Request");

			if (request == null || request.Number == null)
			{
				logger.Debug($"Bad Square Root request: {request} - Number: {request?.Number}");
				return Content(HttpStatusCode.BadRequest, error.get400());
			}

			logger.Info("Processing Square Root Request");
			var response = new Responses.Sqrt();
			response.Square = Math.Sqrt(Convert.ToDouble(request.Number));

			if (response == null || response.Square == null)
			{
				logger.Debug($"Internal error: request: {response} - Square: {response?.Square}");
				return Content(HttpStatusCode.InternalServerError, error.get500());
			}

			logger.Info("Sending Square Root Response");

			return Ok(response);
		}
		#endregion
	}
}
