using CalculatorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace WebCalculator.Controllers
{
    public class JournalController : ApiController
    {
		public static IEnumerable<JournalResponse> JournalList = new List<JournalResponse>();
	}
}
