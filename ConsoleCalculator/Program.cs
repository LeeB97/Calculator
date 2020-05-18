using System;
using RestSharp;
using System.Collections.Generic;
using CalculatorModels;
using System.ComponentModel;

namespace ConsoleCalculator
{
	public class Program
	{
		//global vars
		private const string Header = "X-Evi-Tracking-Id";
		private static Uri URI = new Uri("http://localhost:50691");
		private static IRequest data; //using this for all the classes
		private static string UserInputId;
		private static readonly NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger(); // longs in: \bin\Debug\logs

        #region asks and gets
        private static void askTrackingId()
		{ //Ask user if he wants to save the operation
			Console.WriteLine("Do you want to save the operation? [Y/N] ");
			string YNResponse = Console.ReadLine();

			if (YNResponse == "Y" || YNResponse == "y")
			{
				Console.WriteLine("\nSet up the tracking id: ");
				UserInputId = Console.ReadLine().Trim();
				logger.Info($"User wants to save the operation, Id: {UserInputId}");
			}
		}

		private static void askOptions()
		{
			Console.WriteLine(
				"\nChoose one of the following options:\n" +
				"1 - Add\n" +
				"2 - Subtract\n" +
				"3 - Multiply\n" +
				"4 - Divide\n" +
				"5 - Square root\n" +
                "6 - History\n" +
                "7 - Exit");

            var option = Console.ReadLine();

            switch (option.ToLower())
			{
                case "1":
                case "add":
					logger.Info("Add selected");
					prepareAdd();
					break;
				case "2":
                case "subtract":
                    logger.Info("Subtract selected");
					prepareSubtract();
					break;
				case "3":
                case "multiply":
                    logger.Info("Multiply selected");
					prepareMultiply();
					break;
				case "4":
                case "divide":
                    logger.Info("Divide selected");
					prepareDivide();
					break;
				case "5":
                case "square":
                case "square root":
                    logger.Info("Square Root selected");
					prepareSquareRoot();
					break;
                case "6":
                case "history":
                    checkHistory();
                    break;
                case "7":
                case "exit":
                    return;
                default:
					Console.WriteLine("\nThis option does not exist. Press any key to Restart the application.");
					logger.Debug("Wrong key in  operation type.");
					askOptions();
					break;
			}

            askOptions();
        }

		public static int askNumber(string message, int? counter = null)
		{
			int num;
			string response;
			bool isNumber = false;
			do
			{
				Console.WriteLine($"\n{message}. {counter}");
				response = Console.ReadLine();
				isNumber = Int32.TryParse(response, out num);
				if (!isNumber)
				{
					logger.Debug("Bad data input (not a number).");
					Console.WriteLine("Error! Please type a number");
				}
			} while (!isNumber);

			return num;
		}

		public static int[] getNumbers(int times)
		{
			List<int> numbers = new List<int>();
			for (int i = 1; i <= times; i++)
			{
				numbers.Add(askNumber("Write the number", i));
			}

			return numbers.ToArray();
		}

		#endregion

		#region Prepares
		private static void prepareAdd()
		{
			data = new Requests.Add();
			(data as Requests.Add).Addends = getNumbers(askNumber("How many numbers would you like to use?"));
			logger.Info("Preparing Add Request -> Addends: " + string.Join(" + ", (data as Requests.Add).Addends));

			doRequest("calculator", "add", data);
		}

		private static void prepareSubtract()
		{
			data = new Requests.Sub();
			var numbers = getNumbers(2);
			(data as Requests.Sub).Minuend = numbers[0];
			(data as Requests.Sub).Subtrahend = numbers[1];
			logger.Info($"Preparing Subtract Request -> Minuend: {(data as Requests.Sub).Minuend}, Subtrahend: {(data as Requests.Sub).Subtrahend}");

			doRequest("calculator", "subtract", data);
		}

		private static void prepareMultiply()
		{
			data = new Requests.Mult();
			(data as Requests.Mult).Factors = getNumbers(askNumber("How many numbers would you like to use?"));
			logger.Info("Preparing Add Request -> Factors: " + string.Join(" * ", (data as Requests.Mult).Factors));

			doRequest("calculator", "multiply", data);
		}

		private static void prepareDivide()
		{
			data = new Requests.Div();
			var numbers = getNumbers(2);
			(data as Requests.Div).Dividend = numbers[0];
			(data as Requests.Div).Divisor = numbers[1];
			logger.Info($"Preparing Divide Request -> Dividend: {(data as Requests.Div).Dividend}, Divisor: {(data as Requests.Div).Divisor}");

			doRequest("calculator", "divide", data);
		}

		private static void prepareSquareRoot()
		{
			data = new Requests.Sqrt();
			var numbers = getNumbers(1);
			(data as Requests.Sqrt).Number = numbers[0];
			logger.Info($"Preparing Square Root Request -> Number: {(data as Requests.Sqrt).Number}");

			doRequest("calculator", "squareRoot", data);
		}

        private static void checkHistory()
        {
            var request = new JournalRequest();
            request.Id = UserInputId;

            if (string.IsNullOrEmpty(UserInputId))
            {
                Console.Write("\nID: ");
                request.Id = Console.ReadLine().Trim();
            }

            doRequest("journal", "query", request);
            
        }
        #endregion

        private static void printResponse(IResponse response)
        {
            foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(response))
                Console.Write($"\n {descriptor.Name} = {descriptor.GetValue(response)}");

            Console.WriteLine();
        }

        public static void doRequest(string controller, string type, IRequest data)
		{
			var client = new RestClient(URI);
			var request = new RestRequest($"/api/{controller}/{type}", Method.POST);
			request.RequestFormat = DataFormat.Json;
			request.AddJsonBody(data);

			if (!string.IsNullOrEmpty(UserInputId))
			{
				logger.Info("Header added successfully");
				request.AddHeader(Header, UserInputId);
			}

            var response = client.Execute(request);
            logger.Debug($"Response from '{URI}/api/{controller}{type}': {response.IsSuccessful}.");


            Console.WriteLine($"\nResult: {response.Content}");
        }

        public static void Main(string[] args)
		{
			askTrackingId();
            askOptions();
		}

	}
}
