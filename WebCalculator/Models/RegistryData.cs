using System;
using System.Collections.Generic;
using CalculatorModels;

namespace WebCalculator.Models
{
    public class RegistryData
    {
        public static List<JournalResponse> Registry = new List<JournalResponse>();

        public static JournalResponse byId(string Id)
        {
            return Registry.Find(x => x.Id == Id);
        }

        public static void addRegistry(string Id)
        {
            var newRegistry = new JournalResponse();

            newRegistry.Id = Id;
            newRegistry.Operations = new List<JournalResponse.OperationInfo>();

            Registry.Add(newRegistry);
        }

        public static void addOperation(string Id, string Operation, string Calculation)
        {
            var registry = byId(Id);
            var data = new JournalResponse.OperationInfo();

            data.Operation = Operation;
            data.Calculation = Calculation;
            data.Date = DateTime.UtcNow;

            registry.Operations.Add(data);
        }
    }
}