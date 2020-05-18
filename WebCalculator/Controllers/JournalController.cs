using CalculatorModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebCalculator.Models;

namespace WebCalculator.Controllers
{
    public class JournalController : ApiController
    {
        private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        private readonly ErrorList error = new ErrorList();
        

        [HttpPost]
        public IHttpActionResult Query(JournalRequest request)
        {
            if (request == null || request.Id == null)
            {
                logger.Debug($"Bad Request in Journal: {request}, ID: {request?.Id}");
                return Content(HttpStatusCode.BadRequest, ErrorList.e400);
            }

            var response = RegistryData.byId(request.Id);

            if (response == null)
            {
                logger.Debug($"No Journal Found For The Id: {request?.Id}");
                return Content(HttpStatusCode.NotFound, ErrorList.e500);
            }

            return Ok(response.Operations);
        }
    }
}