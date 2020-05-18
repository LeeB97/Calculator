using System;
using System.Collections.Generic;

namespace CalculatorModels
{
    public class JournalRequest : IRequest
    {
        public string Id { get; set; }
    }

    public class JournalResponse : IResponse
    {
        public string Id { get; set; }
        public List<OperationInfo> Operations { get; set; }

        public class OperationInfo : IResponse
        {
            public string Operation { get; set; }
            public string Calculation { get; set; }
            public DateTime Date { get; set; }
        }
    }
}
